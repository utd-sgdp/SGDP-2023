using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        MovementType _movementType = MovementType.AccelerationWithDrag;

        [SerializeField]
        [Min(0)]
        float _maxVelocity = 500f;

        [SerializeField]
        [Min(0)]
        float _timeToMaxVelocity = 0.25f;
        
        [Header("Look")]
        [SerializeField]
        [Range(0, 1)]
        float  _lookDeadZone = .1f;
        
        Rigidbody _body;
        PlayerInput _input;
        InputAction _moveAction;
        InputAction _lookAction;

        private float _acceleration = 0;
        private float _dragCoefficient = 0;
        private float _velocityMargin = 0.5f;

        // Movement type describes how the player's movement is calculated
        private enum MovementType
        {
            AccelerationWithDrag = 0,
            ConstantVelocity = 1
        }


        static Plane s_groundPlane = new (Vector3.up, Vector3.zero);


        #region MonoBehaviour
        void Awake()
        {
            _body = GetComponent<Rigidbody>();
            
            _input = GetComponent<PlayerInput>();
            _moveAction = _input.actions["Move"];
            _lookAction = _input.actions["Look"];

            // Initiallize movement physics values
            _acceleration = _maxVelocity / _timeToMaxVelocity * lambertW(_maxVelocity / (_velocityMargin));
            _dragCoefficient = _acceleration / _maxVelocity;
        }
        
        void Update()
        {
            Look();
        }
        
        void FixedUpdate()
        {
            Move();
        }
        #endregion

        #region Movement
        /// <summary>
        /// Use the calculated _movementVector to move the player accordingly
        /// </summary>
        void Move(bool Fixed=true)
        {
            // read input
            Vector3 move = _moveAction.ReadValue<Vector2>();

            // convert move input to vector3 and normalize
            move = new Vector3(move.x, 0, move.y);
            move = move.normalized;

            // normalize time
            float normFactor = Fixed ? Time.fixedDeltaTime : Time.deltaTime;

            // The acceleration vector contains how much the velocity will change in a update cycle. This must be declared outside the switch statement
            Vector3 accelerationVector = Vector3.zero;

            // switch on movement type to determine how to calculate the movement equation
            switch (_movementType)
            {
                // This case simulates the player accelerating with drag. It's meant to be the weightiest movement option. Feels like Dusk.
                case MovementType.AccelerationWithDrag:

                    // set drag force
                    accelerationVector = -_body.velocity.normalized * _body.velocity.magnitude * _dragCoefficient * normFactor;

                    // add acceleration if the player is holding a movement input
                    if (move.magnitude > 0)
                        accelerationVector += move * _acceleration * normFactor;

                    // set the velocity to zero or max velocity if it is within the velocity margin
                    if (move.magnitude <= 0 && _body.velocity.magnitude < _velocityMargin)
                        move = Vector3.zero;
                    else if (_maxVelocity - _body.velocity.magnitude < _velocityMargin)
                        move = _body.velocity.normalized * _maxVelocity;

                    // set movement
                    move = _body.velocity + accelerationVector;
                    break;

                // This simulates constant velocity. It is the more responsive option. Very snappy, feels like Hollow Knight.
                case MovementType.ConstantVelocity:
                    move *= _maxVelocity;
                    break;
            }
            
            // apply
            _body.velocity = move;
            
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

        #region Helper Functions

        /// <summary>
        /// Calculates the lambertW function for float
        /// </summary>
        private float lambertW(float x)
        {
            if (x < -Math.Exp(-1))
                throw new Exception("The LambertW-function is not defined for " + x + ".");

            int amountOfIterations = Mathf.Max(4, (int)Mathf.Ceil(Mathf.Log10(x) / 3));
            float w = 3 * Mathf.Log(x + 1) / 4;

            for (int i = 0; i < amountOfIterations; i++)
                w = w - (w * Mathf.Exp(w) - x) / (Mathf.Exp(w) * (w + 1) - (w + 2) * (w * Mathf.Exp(w) - x) / (2 * w + 2));

            return w;
        }

        #endregion
    }
}
