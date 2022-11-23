using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Camera
{
    [RequireComponent(typeof(Collider))]
    public class RoomTrigger : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent OnRoomEnter; 
        public UnityEvent OnRoomExit; 
        
        static int s_playerLayer;
        Collider _collider;
        
        void Awake()
        {
            s_playerLayer = LayerMask.NameToLayer("Player");
            _collider = GetComponent<Collider>();
        }

        void OnTriggerEnter(Collider other)
        {
            // exit, collision was not player
            if (!IsTarget(other.gameObject)) return;
            
            // player entered room
            LevelCameraController.Instance.SetConfiner(_collider);
            OnRoomEnter?.Invoke();
        }

        void OnTriggerExit(Collider other)
        {
            // exit, collision was not player
            if (!IsTarget(other.gameObject)) return;
            
            OnRoomExit?.Invoke();
        }

        /// <summary>
        /// Verifies <param name="other"></param> is on the <see cref="s_playerLayer"/>
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        static bool IsTarget(GameObject other)
        {
            return s_playerLayer == other.layer;
        }
    }
}