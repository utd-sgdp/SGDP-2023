using Game.Play.Camera;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Agent.Tree
{
    public class AIAgent : MonoBehaviour
    {
        private Vector3 _RoomTriggerPosition;
        private NavMeshAgent _agent;
        private bool _shouldWander;
        private bool _isWaitingToWander;

        [SerializeField]
        public BehaviourTree Tree;
        [Tooltip("Whether the AI should wander within its room before the player enters the room.")]
        public bool willWander;
        [Tooltip("Time in second between when the AI completes a wander path and when it starts a new one")]
        public float delayBetweenWanders;
        public RoomTrigger RoomTrigger;
        
        
        void Start()
        {
            if (Tree == null)
            {
                Debug.LogError($"No BehaviourTree was provided to { name }.");
                this.enabled = false;
                return;
            }

            // case: use BehaviourTree ScriptableObject
            if (GetType() == typeof(AIAgent))
            {
                // create runtime tree
                Tree = Tree.Clone();
            }
            // case: create BehaviourTree at runtime though code 
            else
            {
                Tree = CreateTree();
                if (Tree == null)
                {
                    Debug.LogError($"{ GetType().BaseType } must implement a { typeof(BehaviourTree) } in CreateTree().");
                    this.enabled = false;
                    return;
                }
            }
            
            Tree.Bind(gameObject, transform);


            if (_agent == null)
            {
                _agent = GetComponent<NavMeshAgent>();
            }

            if (RoomTrigger != null)
            {
                _RoomTriggerPosition = RoomTrigger.GetComponent<Transform>().position;
                RoomTrigger.OnRoomEnter.AddListener(StopWandering);
            }

            _shouldWander = willWander;

        }

        void Update()
        {
            if (_shouldWander)
            {
                if (!_isWaitingToWander)
                {
                    StartCoroutine(Wander());
                }
            }

            else
            { 
                State treeState = Tree.Update();
                if (treeState is State.Running) return;

                // root node finished execution
                // stop execution
                this.enabled = false;
            }
        }

        /// <summary>
        /// Should be overriden to implement <see cref="BehaviourTree"/>'s in code. This will be ignored if
        /// <see cref="Tree"/> has already been assigned.
        /// </summary>
        /// <returns> The <see cref="BehaviourTree"/> to be used by this <see cref="AIAgent"/>. </returns>
        protected virtual BehaviourTree CreateTree() => null;

        private IEnumerator Wander()
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                for (byte i = 0; i < 30; i++) //Try to find a valid point to wander to 30 times
                {
                    Vector3 randomPoint = GetRandomPointInRoomTrigger();
                    NavMeshHit hit;

                    if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, _agent.areaMask))
                    {
                        _isWaitingToWander = true;
                        yield return new WaitForSeconds(delayBetweenWanders); //Wait before wandering
                        _isWaitingToWander = false;

                        if (_shouldWander) //Make sure that we should still wander after waiting
                        {
                            _agent.destination = hit.position;
                        }

                        break;
                    }
                }
            }
        }

        private Vector3 GetRandomPointInRoomTrigger()
        {
            Vector3 RoomTriggerSize = RoomTrigger.Collider.bounds.size;
            float x = UnityEngine.Random.Range(_RoomTriggerPosition.x - RoomTriggerSize.x / 2, _RoomTriggerPosition.x + RoomTriggerSize.x / 2);
            float z = UnityEngine.Random.Range(_RoomTriggerPosition.z - RoomTriggerSize.z / 2, _RoomTriggerPosition.z + RoomTriggerSize.z / 2);
            return new Vector3(x, 0, z);
        }

        private void StopWandering()
        {
            _shouldWander = false;
            RoomTrigger.OnRoomEnter.RemoveListener(StopWandering);
        }
        
        #if UNITY_EDITOR
        [Button(Spacing = 15)]
        public void ValidateDependencies()
        {
            bool isValid = true;
            BehaviourTree.Traverse(Tree.RootNode, node =>
            {
                foreach (string type in node.GetDependencies())
                {
                    Type t = Type.GetType(type);
                    if (t == null)
                    {
                        Debug.LogWarning($"'{ node.name }' has unknown dependency type: { type }.");
                        continue;
                    }

                    var component = gameObject.GetComponentInChildren(t);
                    if (component)
                    {
                        continue;
                    }

                    Debug.LogError($"'{ gameObject.name }' is missing a component: { type }.");
                    isValid = false;
                }
            });

            if (isValid)
            {
                Debug.Log("SUCCESS: this object has all known dependencies.");
            }
        }
        #endif
    }
}
