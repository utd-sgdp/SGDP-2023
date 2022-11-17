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
        [SerializeField] protected float _despawnDelay = 20f;
        [SerializeField] protected float _despawnTime = 1.5f;

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
                StartCoroutine(DespawnPieces(_despawnTime, _despawnDelay));
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

        //Despawn objects after moving below the floor
        IEnumerator DespawnPieces(float duration, float delay)
        {
            yield return new WaitForSeconds(delay);

            float timeElapsed = 0;
            Vector3 startPosition = transform.position;
            Vector3 despawnPosition = startPosition + new Vector3(0, -10, 0);

            while (timeElapsed < duration)
            {
                transform.position = Vector3.Lerp(startPosition, despawnPosition, timeElapsed / duration);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

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
