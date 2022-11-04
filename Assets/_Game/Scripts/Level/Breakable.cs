using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Level
{
    public class Breakable : MonoBehaviour
    {
        [SerializeField, HighlightIfNull]
        protected GameObject _unbrokenObject;

        [SerializeField, HighlightIfNull]
        protected GameObject _brokenObject;

        [SerializeField] protected float _breakingThreshold = 1f;
        [SerializeField] protected float _physicsDisableDelay = 5f;
        [SerializeField] protected float _despawnDelay = 30f;

        [Header("Events")]
        public UnityEvent<TransformData> OnBreak;

        void Start()
        {
            _unbrokenObject.SetActive(true);
            _brokenObject.SetActive(false);
        }

        void OnCollisionEnter(Collision collision)
        {
            // exit, collision is too small
            if (!(collision.impulse.magnitude > _breakingThreshold)) return;

            // disable rigidbody for unbroken object
            Destroy(GetComponent<Rigidbody>());
            
            // swap broken/unbroken
            _unbrokenObject.SetActive(false);
            _brokenObject.SetActive(true);
            
            OnBreak?.Invoke(transform);

            if (_physicsDisableDelay >= 0)
            {
                StartCoroutine(Coroutines.WaitThen(_physicsDisableDelay, StopPhysics));
            }

            if (_despawnDelay >= 0)
            {
                StartCoroutine(Coroutines.WaitThen(_despawnDelay, DespawnPieces));
            }
        }

        void StopPhysics()
        {
            foreach (Transform child in _brokenObject.transform)
            {
                var rb = child.gameObject.GetComponent<Rigidbody>(); 
                Destroy(rb);
            }
        }

        void DespawnPieces()
        {
            Destroy(_brokenObject);
            Destroy(_unbrokenObject);
        }
        
        #if UNITY_EDITOR
        [Button(Label = "Add Rigidbodies")]
        public void AddRigidbodies()
        {
            Undo.RecordObject(gameObject, "Breakable (Add Rigidbodies)");
            foreach (var piece in _brokenObject.GetComponentsInChildren<MeshFilter>())
            {
                Rigidbody rb = piece.GetComponent<Rigidbody>();
                if (rb) continue;

                Undo.AddComponent<Rigidbody>(piece.gameObject);
            }
        }

        [Button(Label = "Add Mesh Colliders")]
        public void AddColliders()
        {
            Undo.RecordObject(gameObject, "Breakable (Add Mesh Colliders)");
            foreach (var piece in _brokenObject.GetComponentsInChildren<MeshFilter>())
            {
                Collider col = piece.GetComponent<Collider>();
                if (col) continue;

                MeshCollider m = Undo.AddComponent<MeshCollider>(piece.gameObject);
                m.sharedMesh = piece.sharedMesh;
            }
        }
        #endif
    }
}
