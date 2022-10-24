using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Game.Camera
{
    //This class sets up and handles all the necessary information for the virtual camera to function properly
    public class CameraController : MonoBehaviour
    {
        // references to objects the script needs
        private GameObject _player = null;
        private CinemachineVirtualCamera _camera = null;
        private CinemachineConfiner _confiner = null;

        // When the script enters the game, look for the necessary object references
        void Awake() {
            _camera = GetComponentInChildren<CinemachineVirtualCamera>();
            _confiner = GetComponentInChildren<CinemachineConfiner>();
            _player = GameObject.FindWithTag("Player"); //Find a GameObject with the "player" tag. Only the player character should have this tag.
            // Check to make sure object references were found
            if(_player == null) {
                Exception e = new ArgumentNullException("Null player reference found!");
                Debug.LogException(e);
                return;
            }
            if (_camera == null) {
                Exception e = new ArgumentNullException("Null camera reference found!");
                Debug.LogException(e);
                return;
            }
            if (_confiner == null) {
                Exception e = new ArgumentNullException("Null confiner reference found!");
                Debug.LogException(e);
                return;
            }
            _camera.m_Follow = _player.transform;
        }

        /// <summary>
        /// Set the Cinemachine camera to the newly entered room. Will be called by a plane upon calling OnTriggerEnter()
        /// </summary>
        /// <param name="colision">The collision object that will become the new bound volume</param>
        public void SetBoundVolume(Collider collision) {
            _confiner.m_BoundingVolume = collision;
        }
    }
}
