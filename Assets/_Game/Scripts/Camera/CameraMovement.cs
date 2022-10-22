using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Game.Camera {

    [RequireComponent(typeof(BoxCollider))]
    public class CameraMovement : MonoBehaviour {

        // Reference to the virtual camera that the plane owns
        CinemachineVirtualCamera _camera;

        private void Awake() {
            // Get the camera reference at the start
            _camera = GetComponentInChildren<CinemachineVirtualCamera>();
        }

        private void OnTriggerEnter(Collider other) {
            // Check that we're detecting the player by checking the collider's tag for the "Player" tag
            if (other.gameObject.tag == "Player") {
                _camera.gameObject.SetActive(true); // If the player is in the plane's area, activate its camera
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.gameObject.tag == "Player") {
                _camera.gameObject.SetActive(false); // If the player leaves the plane's area, deactivate its camera
            }
        }
    }
}