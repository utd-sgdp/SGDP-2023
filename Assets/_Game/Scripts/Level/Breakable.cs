using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Level
{
    public class Breakable : MonoBehaviour
    {
        [SerializeField, HighlightIfNull]
        protected GameObject _brokenObject;

        [SerializeField, HighlightIfNull]
        protected GameObject _unbrokenObject;

        [SerializeField] protected float _breakingThreshold = 1f;
        [SerializeField] protected float _physicsDisableDelay = 5f;
        [SerializeField] protected float _despawnDelay = 20f;
        [SerializeField] protected float _despawnTime = 1.5f;

        [Header("Events")]
        public UnityEvent<TransformData> OnBreak;

        void Start()
        {
            _brokenObject.SetActive(true);
            _unbrokenObject.SetActive(false);
        }

        void OnCollisionEnter(Collision collision)
        {
            // exit, collision is too small
            if (!(collision.impulse.magnitude > _breakingThreshold)) return;

            // disable rigidbody for unbroken object
            Destroy(GetComponent<Rigidbody>());
            
            // swap broken/unbroken
            _brokenObject.SetActive(false);
            _unbrokenObject.SetActive(true);
            
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
            foreach (Transform child in _unbrokenObject.transform)
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

            Destroy(_unbrokenObject);
            Destroy(_brokenObject);
        }
    }
}
