using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Camera
{
    //This class is used by rooms to detect the presence of the player and adjust the virtual camera's confinement box
    public class RoomDetector : MonoBehaviour
    {
        private CameraController _camera;
        private BoxCollider _box;
        private void Awake() {
            _camera = GameObject.FindWithTag("Camera").GetComponent<CameraController>(); // get the camera object and access its script component
            _box = GetComponentInChildren<BoxCollider>(); // get a reference to the room's boxed area.
        }

        private void OnTriggerEnter(Collider other) {
            if(other.gameObject.transform.parent.tag == "Player") { // check to see if the object collided with the collider is the player
                _camera.SetBoundVolume(_box); // set bound volume to this room's box collider
            }
        }
    }
}
