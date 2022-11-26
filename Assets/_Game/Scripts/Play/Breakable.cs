using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Utility;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Play
{
    public class Breakable : MonoBehaviour
    {
        [SerializeField, HighlightIfNull]
        GameObject _unbrokenObject;

        [SerializeField, HighlightIfNull]
        GameObject _brokenObject;

        [SerializeField]
        float _breakingThreshold = 1f;
        
        [SerializeField]
        [Min(0)]
        float _despawnDelay = 20f;
        
        [SerializeField]
        [Min(0)]
        float _despawnTime = 1.5f;

        [Header("Events")]
        public UnityEvent<TransformData> OnBreak;

        // data collection at build time
        [SerializeField, HideInInspector] PhysicsComponents[] _piecesWithPhysics;
        [SerializeField, HideInInspector] bool _noPiecesHavePhysics;
        [SerializeField, HideInInspector] bool _allPiecesHavePhysics;

        Rigidbody _rb;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            if (_rb == null) Debug.LogError($"{ name } is a missing a RigidBody component.");
            
            _unbrokenObject.SetActive(true);
            _brokenObject.SetActive(false);
        }

        void OnCollisionEnter(Collision collision)
        {
            // exit, collision is too small
            if (!(collision.impulse.magnitude > _breakingThreshold)) return;
            
            Break();
        }

        public void Break()
        {
            // disable rigidbody for unbroken object
            Destroy(_rb);
            
            // swap broken/unbroken
            Destroy(_unbrokenObject);
            _brokenObject.SetActive(true);
            OnBreak?.Invoke(transform);

            // wait, then perform despawn animation
            StartCoroutine(DespawnPieces());
        }

        void CollectReferences()
        {
            if (_brokenObject == null)
            {
                Debug.LogError($"{ name } is missing a reference to a break object.");
                return;
            }
            
            Rigidbody[] rbs = _brokenObject.GetComponentsInChildren<Rigidbody>();
            _piecesWithPhysics = new PhysicsComponents[rbs.Length];
            _allPiecesHavePhysics = rbs.Length == _brokenObject.transform.childCount;
            _allPiecesHavePhysics = rbs.Length == 0;
            
            for (int i = 0; i < rbs.Length; i++)
            {
                Rigidbody rb = rbs[i];
                _piecesWithPhysics[i] = new PhysicsComponents
                {
                    GameObject = rb.gameObject,
                    RB = rb,
                    Colliders = rb.GetComponents<Collider>(),
                };
            }
        }

        void StopPhysics()
        {
            foreach (PhysicsComponents child in _piecesWithPhysics)
            {
                Destroy(child.RB);
                foreach (Collider col in child.Colliders)
                {
                    col.enabled = false;
                }
            }
        }

        void DestroyPieces()
        {
            foreach (PhysicsComponents child in _piecesWithPhysics)
            {
                Destroy(child.GameObject);
            }
        }

        // wait, animate, despawn
        IEnumerator DespawnPieces()
        {
            // case: no pieces have physics
            // no animation/despawn necessary
            if (_noPiecesHavePhysics)
            {
                yield break;
            }
            
            // wait
            yield return new WaitForSeconds(_despawnDelay);

            // make group for pieces to animate
            GameObject animationGroup = Pool.CheckOutGameObject();
            Transform groupTransform = animationGroup.transform;
            animationGroup.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            
            // move pieces to group
            foreach (var child in _piecesWithPhysics)
            {
                child.GameObject.transform.SetParent(groupTransform);
            }
            
            StopPhysics();
            yield return DespawnAnimation(groupTransform);

            // despawn
            foreach (Transform child in groupTransform)
            {
                Destroy(child.gameObject);
            }
            Pool.CheckInGameObject(animationGroup);

            if (!_allPiecesHavePhysics) yield break;

            // there's nothing left here, just go destroy this too
            Destroy(gameObject, 0.01f);
        }

        IEnumerator DespawnAnimation(Transform target)
        {
            float timeElapsed = 0;
            Vector3 startPosition = target.position;
            Vector3 despawnPosition = startPosition + new Vector3(0, -10, 0);

            while (timeElapsed < _despawnTime)
            {
                target.position = Vector3.Lerp(startPosition, despawnPosition, timeElapsed / _despawnTime);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
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

        [System.Serializable]
        struct PhysicsComponents
        {
            public GameObject GameObject;
            public Rigidbody RB;
            public Collider[] Colliders;
        }

        /// <summary>
        /// At build, automatically collect component references
        /// </summary>
        class PhysicsComponentsCollector : IProcessSceneWithReport
        {
            public int callbackOrder { get; }
            public void OnProcessScene(Scene scene, BuildReport report)
            {
                List<GameObject> gameObjects = scene.GetRootGameObjects().ToList();
                gameObjects.Traverse(go =>
                {
                    Breakable breakable = go.GetComponent<Breakable>();
                    if (!breakable) return;
                    
                    breakable.CollectReferences();
                    foreach (var trans in go.GetComponentsInChildren<Transform>(includeInactive: true))
                    {
                        trans.gameObject.layer = LayerMask.NameToLayer("Breakable");
                    }
                });
            }
        }
    }
}
