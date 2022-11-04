using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    }
}
