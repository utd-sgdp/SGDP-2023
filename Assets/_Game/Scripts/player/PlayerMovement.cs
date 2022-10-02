using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        [Min(0)]
        float _movementSpeed = 5f;
        Vector3 _movementVector = Vector3.zero;
        Vector2 _aim;
        Rigidbody _body;
        PlayerInput _input;

        /// <summary>
        /// Initialize the PlayerInput object on startup
        /// </summary>
        void Awake()
        {
            _input = GetComponent<PlayerInput>();
        }

        /// <summary>
        /// Method called when object becomes active and enabled
        /// </summary>
        void OnEnable()
        {
            SubscribeInputs();
        }

        /// <summary>
        /// Sets up and enables the PlayerInput actions. Abstracts the process to OnEnable
        /// </summary>
        void SubscribeInputs() 
        {
            _input.actions["Move"].performed += OnMove;
        }

        /// <summary>
        /// Method called when object is disabled for destroyed
        /// </summary>
        void OnDisable()
        {
            UnsubscribeInputs();
        }

        /// <summary>
        /// Disables PlayerInput actions. Abstracts the process to OnDisable
        /// </summary>
        void UnsubscribeInputs()
        {
            _input.actions["Move"].performed -= OnMove;
        }

        void OnMove(InputAction.CallbackContext context) 
        {
            _movementVector = context.ReadValue<Vector3>();
        }

        void Start()
        {
            _body = GetComponent<Rigidbody>();
        }

        void Update()
        {
         
        }

        void FixedUpdate()
        {
            MovePlayer();
        }

        /// <summary>
        /// Get current position of the mouse (if using mouse & keyboard) and calculate the angle the player should rotate to. Works well with WASD keys.
        /// </summary>
        void GetMousePosition()
        {
            /*Ray ray = Camera.main.ScreenPointToRay(_aim);
            Plane s_plane = new (Vector3.up, Vector3.zero);
            
            if (s_plane.Raycast(ray, out float rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);
            }*/
        }

        void LookAt(Vector3 lookPoint)
        {
            Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
            transform.LookAt(heightCorrectedPoint);
        }

        /// <summary>
        /// Get keyboard from the player from keyboard or controller. Override mouse angle if using keyboard only or controller
        /// </summary>
        void GetPlayerInput()
        {
            
        }

        /// <summary>
        /// Use the calculated _movementVector to move the player accordingly
        /// </summary>
        void MovePlayer()
        {
            _body.velocity = new Vector3(_movementVector.x * _movementSpeed, 0f, _movementVector.z * _movementSpeed);
        }
    }
}
