using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player {

    public class PlayerMovement : MonoBehaviour {
        // Enter public variables here
        public float speed = 5f; // variable number that dictates the speed in which the player character moves
        public PlayerInputActions playerControls; // Input control system for player movement controls
       
        // Enter private variables here
        private InputAction move;
        private Vector3 movementVector = Vector3.zero;
        private Rigidbody body;

        /// <summary>
        /// Initialize the playerControls object on startup
        /// </summary>
        private void Awake() {
            playerControls = new PlayerInputActions();
        }

        /// <summary>
        /// Required method for new Unity input system. Activates the playerControls input action system.
        /// </summary>
        private void OnEnable() {
            move = playerControls.Player.Move;
            move.Enable();
        }

        /// <summary>
        /// Required method for new Unity input system. Disables the playerControls input action system
        /// </summary>
        private void OnDisable() {
            move.Disable();
        }

        // Start is called before the first frame update
        void Start() {
            body = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update() {
            // Execute player movement tasks
            GetMousePosition();
            GetPlayerInput();
            SetOrientation();
            MovePlayer();
        }

        /// <summary>
        /// Get current position of the mouse (if using mouse & keyboard) and calculate the angle the player should rotate to
        /// </summary>
        private void GetMousePosition() {

        }

        /// <summary>
        /// Get keyboard from the player from keyboard or controller. Override mouse angle if using keyboard only or controller
        /// </summary>
        private void GetPlayerInput() {
            movementVector = move.ReadValue<Vector3>(); // read the player input for movement direction
        }

        /// <summary>
        /// Use the calculated angle and rotate the player accordingly
        /// </summary>
        private void SetOrientation() {

        }

        /// <summary>
        /// Use the calculated movementVector to move the player accordingly
        /// </summary>
        private void MovePlayer() {
            body.velocity = new Vector3(movementVector.x * speed, 0f, movementVector.z * speed);
        }
    }
}
