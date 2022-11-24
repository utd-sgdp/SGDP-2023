using System;
using System.Collections;
using Game.Animation;
using Game.Play.Items.Statistics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour, IStatTarget, IMovementProvider
    {
        #region Events
        public Action<Vector2> OnMove;
        public void SubscribeToOnMove(Action<Vector2> callback) => OnMove += callback;
        public void UnsubscribeToOnMove(Action<Vector2> callback) => OnMove -= callback;
        #endregion
        
        #region Inspector Variables
        [Header("Movement")]
        [SerializeField]
        MovementType _movementType = MovementType.AccelerationWithDrag;

        [SerializeField]
        [Min(0)]
        float _maxVelocity = 15f;

        [SerializeField]
        [Min(0)]
        float _timeToMaxVelocity = 0.25f;

        public float Multiplier = 1f;

        [SerializeField, ReadOnly]
        Vector3 _movement;

        [Header("Dash")]
        [SerializeField]
        [Min(0)]
        float _dashLength = 5f;

        [SerializeField]
        [Min(0)]
        float _dashDuration = 0.05f;

        [SerializeField]
        [Min(0)]
        float _dashCoolDown = 0.5f;

        [Header("Look")]
        [SerializeField]
        [Range(0, 1)]
        float  _lookDeadZone = .1f;
        #endregion

        #region Private Variables
        bool _canMove = true;
        bool Dashing
        {
            get => _dashing != null;
        }
        Coroutine _dashing;
        
        Rigidbody _body;
        PlayerInput _input;
        InputAction _moveAction;
        InputAction _lookAction;
        InputAction _dashAction;

        float _dashVelocity;
        float _acceleration;
        float _dragCoefficient;
        const float VELOCITY_MARGIN = 0.5f;
        const float VELOCITY_MARGIN_SQ = VELOCITY_MARGIN * VELOCITY_MARGIN;

        static Plane s_groundPlane = new (Vector3.up, Vector3.zero);
        #endregion

        #region MonoBehaviour
        void Awake()
        {
            _body = GetComponent<Rigidbody>();
            InitializeMoveData();
        }

        void OnEnable()
        {
            _input = GetComponent<PlayerInput>();
            _moveAction = _input.actions["Move"];
            _lookAction = _input.actions["Look"];
            _dashAction = _input.actions["Dash"];

            SubInput();
        }

        void OnDisable() => UnsubInput();

        void Update()
        {
            Look();
        }
        
        void FixedUpdate()
        {
            Vector3 direction = GetMoveDirection();
            Move(direction);
        }

        void OnValidate() => InitializeMoveData();
        #endregion
        
        #region Input
        void SubInput() => _dashAction.performed += OnDash;
        void UnsubInput() => _dashAction.performed -= OnDash;

        Vector3 GetMoveDirection()
        {
            Vector3 direction = _moveAction.ReadValue<Vector2>();
            direction = new Vector3(direction.x, 0, direction.y);
            return direction.normalized;
        }
        #endregion

        #region Movement
        void InitializeMoveData()
        {
            _acceleration = _maxVelocity / _timeToMaxVelocity * Utility.Math.LambertW(_maxVelocity / VELOCITY_MARGIN);
            _dragCoefficient = _acceleration / _maxVelocity;
            _dashVelocity = _dashLength / _dashDuration;
        }
        
        /// <summary>
        /// Move in <see cref="direction"/> with acceleration curve specified by <see cref="_movementType"/>.
        /// </summary>
        /// <param name="direction"> normalized, local-space direction vector </param>
        void Move(Vector3 direction)
        {
            // exit, the player is not able to move
            if (!_canMove) return;

            Vector3 nVelocity;
            switch (_movementType)
            {
                case MovementType.AccelerationWithDrag:
                    nVelocity = CalculateVelocityWithDrag(direction);
                    break;
                
                default:
                case MovementType.ConstantVelocity:
                    nVelocity = CalculateConstVelocity(direction);
                    break;
            }

            _body.velocity = nVelocity * Multiplier;
            
            // calculate movement relative to the direction we are facing
            nVelocity = Quaternion.Euler(0, -transform.rotation.eulerAngles.y, 0) * nVelocity;
            OnMove?.Invoke(new Vector2(nVelocity.x, nVelocity.z));
        }
        
        /// <summary>
        /// Velocity is calculated with no acceleration.
        /// </summary>
        /// <param name="direction"> normalized, local-space direction </param>
        /// <returns> new velocity </returns>
        Vector3 CalculateConstVelocity(Vector3 direction)
        {
            return _maxVelocity * direction;
        }
        
        /// <summary>
        /// Velocity is calculated with acceleration depending on input value and previous velocity.
        /// </summary>
        /// <param name="direction"> normalized, local-space direction </param>
        /// <returns> new velocity </returns>
        Vector3 CalculateVelocityWithDrag(Vector3 direction)
        {
            Vector3 currentVelocity = _body.velocity;
            float   currentSpeed    = currentVelocity.magnitude;
                
            // calculate drag force
            Vector3 acceleration = -currentVelocity.normalized * (currentSpeed * _dragCoefficient * Time.fixedDeltaTime);

            // add acceleration, the player is holding a movement input
            bool isInputMovement = direction.magnitude > float.Epsilon;
            if (isInputMovement)
            {
                acceleration += direction * (_acceleration * Time.fixedDeltaTime);
            }

            Vector3 nVelocity = _body.velocity + acceleration;
            return nVelocity.sqrMagnitude > VELOCITY_MARGIN_SQ ? nVelocity : Vector3.zero;
        }
        #endregion

        #region Dash
        void OnDash(InputAction.CallbackContext context)
        {
            Vector3 direction = GetMoveDirection();
            AttemptDash(direction);
        }
        
        /// <summary>
        /// Attempts to dash in <see cref="direction"/>.
        /// </summary>
        /// <param name="direction"> Normalized, local-space direction. </param>
        /// <returns> Success of the operation. </returns>
        public bool AttemptDash(Vector3 direction)
        {
            // we are currently dashing, exit
            if (Dashing) return false;

            _canMove = false;
            _dashing = StartCoroutine(Dash(direction));
            return true;
        }
        
        IEnumerator Dash(Vector3 direction)
        {
            // Set the velocity of the player to the velocity of the dash
            _body.velocity = direction.normalized * _dashVelocity;

            yield return new WaitForSeconds(_dashDuration);

            // ReSharper disable once Unity.InefficientPropertyAccess
            _body.velocity = Vector3.zero;
            _canMove = true;

            yield return new WaitForSeconds(_dashCoolDown);

            _dashing = null;
        }
        #endregion

        #region Aim
        /// <summary>
        /// Rotates the player with input-device specific logic
        /// </summary>
        void Look()
        {
            // exit if no input
            if (_lookAction.activeControl == null) return;
            
            // check device type
            InputDevice device = _lookAction.activeControl.device;
            bool isMouse = device is Pointer;
            
            // read input
            Vector2 lookValue = _lookAction.ReadValue<Vector2>();

            // choose Look method based on device type
            if (isMouse)
            {
                LookMouse(lookValue);
            }
            else
            {
                LookController(lookValue);
            }
        }

        /// <summary>
        /// Rotates the player to look toward the mouse.
        /// </summary>
        /// <param name="position"> Mouse position in screen space. </param>
        void LookMouse(Vector2 position)
        {
            // get mouse position in world
            Ray ray = _input.camera.ScreenPointToRay(position);
            s_groundPlane.Raycast(ray, out float distance);
            Vector3 mouseInWorld = ray.GetPoint(distance);

            // rotate toward mouse
            mouseInWorld.y = transform.position.y;
            transform.LookAt(mouseInWorld);
        }

        /// <summary>
        /// Rotates the player to look in the given direction.
        /// </summary>
        /// <param name="direction"> Gamepad, or Joystick, direction. </param>
        void LookController(Vector2 direction)
        {
            if (direction.sqrMagnitude < _lookDeadZone) return;
            
            Vector3 worldDir = new Vector3(direction.x, 0, direction.y);
            transform.forward = worldDir;
        }
        #endregion
        
        #region Multipliers
        public void OnStatChange(Stat stat)
        {
            Multiplier = stat.Value;
            print($"Changed speed multiplier to { stat.Value }.");
        }
        #endregion
    }

    #region Enums
    /// <summary>
    /// Movement type describes how the player's movement is calculated
    /// </summary>
    public enum MovementType
    {
        AccelerationWithDrag = 0,
        ConstantVelocity = 1,
    }
    #endregion
}
