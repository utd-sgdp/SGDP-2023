using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField]
        [Min(0)]
        float _movementSpeed = 500f;
        
        [Header("Look")]
        [SerializeField]
        [Range(0, 1)]
        float  _lookDeadZone = .1f;
        
        Rigidbody _body;
        PlayerInput _input;
        InputAction _moveAction;
        InputAction _lookAction;
        
        static Plane s_groundPlane = new (Vector3.up, Vector3.zero);

        #region MonoBehaviour
        void Awake()
        {
            _body = GetComponent<Rigidbody>();
            
            _input = GetComponent<PlayerInput>();
            _moveAction = _input.actions["Move"];
            _lookAction = _input.actions["Look"];
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
            Vector2 move = _moveAction.ReadValue<Vector2>();
            
            // apply speed multiplier and normalize time
            float normFactor = Fixed ? Time.fixedDeltaTime : Time.deltaTime;
            move *= _movementSpeed * normFactor;
            
            // apply
            _body.velocity = new Vector3(move.x, 0f, move.y);
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
    }
}
