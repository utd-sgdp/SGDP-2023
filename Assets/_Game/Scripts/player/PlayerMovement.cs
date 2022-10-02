using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        [Min(0)]
        float _movementSpeed = 5f;

        PlayerInputActions playerControls;
        InputAction move;
        
        Vector3 movementVector = Vector3.zero;
        Vector2 aim;
        Rigidbody body;

        /// <summary>
        /// Initialize the playerControls object on startup
        /// </summary>
        void Awake()
        {
            playerControls = new PlayerInputActions();
        }

        /// <summary>
        /// Required method for new Unity input system. Activates the playerControls input action system.
        /// </summary>
        void OnEnable()
        {
            move = playerControls.Player.Move;
            move.Enable();
        }

        /// <summary>
        /// Required method for new Unity input system. Disables the playerControls input action system
        /// </summary>
        void OnDisable()
        {
            move.Disable();
        }

        void Start()
        {
            body = GetComponent<Rigidbody>();
        }

        void Update()
        {
            // Execute player movement tasks
            GetMousePosition();
            GetPlayerInput();
            LookAt(aim);
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
            Ray ray = Camera.main.ScreenPointToRay(aim);
            Plane s_plane = new (Vector3.up, Vector3.zero);
            
            if (s_plane.Raycast(ray, out float rayDistance))
            {
                Vector3 point = ray.GetPoint(rayDistance);
                LookAt(point);
            }
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
            movementVector = move.ReadValue<Vector3>(); // read the player input for movement direction
        }

        /// <summary>
        /// Use the calculated movementVector to move the player accordingly
        /// </summary>
        void MovePlayer()
        {
            body.velocity = new Vector3(movementVector.x * _movementSpeed, 0f, movementVector.z * _movementSpeed);
        }
    }
}
