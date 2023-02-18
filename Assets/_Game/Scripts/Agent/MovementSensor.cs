using System;
using System.Collections;
using Game.Agent.Tree;
using Game.Animation;
using Game.Play.Camera;
using Game.Play.Items.Statistics;
using Game.Play.Level;
using Game.Utility;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Game.Agent
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementSensor : MonoBehaviour, IMovementProvider, IStatTarget
    {
        public bool IsWandering { get; private set; }
        
        #region Events
        public Action<Vector2> OnMove;
        public void SubscribeToOnMove(Action<Vector2> movement) => OnMove += movement;
        public void UnsubscribeToOnMove(Action<Vector2> movement) => OnMove -= movement;
        #endregion

        [SerializeField]
        [Tooltip("Whether the AI should wander within its room before the player enters the room.")]
        bool _wanderAtStart = true;

        [SerializeField]
        [Tooltip("Time in second between when the AI completes a wander path and when it starts a new one")]
        Optional<Vector2> _delayBetweenWanders = new(enabled: true, value: new Vector2(1, 3));
        
        #if UNITY_EDITOR
        [SerializeField, ReadOnly]
        float _multiplier = 1;
        #endif
        
        NavMeshAgent _navAgent;
        AIAgent _aiAgent;
        RoomTrigger _roomTrigger;
        
        float _baseMove;
        float _baseTurn;

        Coroutine _wanderLoop;

        const int MAX_WANDER_SAMPLES = 30;

        void Awake()
        {
            _navAgent = GetComponent<NavMeshAgent>();
            _aiAgent = GetComponentInParent<AIAgent>();
            _roomTrigger = GetComponentInParent<Room>().Trigger;
            
            _baseMove = _navAgent.speed;
            _baseTurn = _navAgent.angularSpeed;

            if (_roomTrigger == null && _wanderAtStart)
            {
                throw new NullReferenceException(
                    $"Unable to find {name}'s RoomTrigger. Enemies should be children of a room, if they are " +
                    $"supposed to wander at anytime."
                );
            }
        }
        
        void OnEnable()
        {
            if (_roomTrigger) _roomTrigger.OnRoomEnter.AddListener(StopWander);
        }

        void Start()
        {
            if (_wanderAtStart) SetWander(true);
        }
        
        void OnDisable()
        {
            if (_roomTrigger) _roomTrigger.OnRoomEnter.RemoveListener(StopWander);
        }

        void Update()
        {
            if (_navAgent.isStopped)
            {
                OnMove?.Invoke(Vector2.zero);
                return;
            }

            // calculate movement relative to the direction we are facing
            Vector3 velocity = _navAgent.velocity;
            velocity = Quaternion.Euler(0, -transform.rotation.eulerAngles.y, 0) * velocity;
            
            Vector2 move = new Vector2(velocity.x, velocity.z);
            OnMove?.Invoke(move);
        }

        public void OnStatChange(Stat stat)
        {
            #if UNITY_EDITOR
            _multiplier = stat.Value;
            #endif
            
            _navAgent.speed = _baseMove * stat.Value;
            _navAgent.angularSpeed = _baseTurn * stat.Value;
        }

        public void SetWander(bool value)
        {
            // exit, no change is necessary
            if (IsWandering == value) return;
            
            if (value) StartWander();
            else StopWander();
        }

        void StartWander()
        {
            IsWandering = true;
            _aiAgent.enabled = false;

            // start wander loop
            _wanderLoop = StartCoroutine(WanderLoop());
        }

        void StopWander()
        {
            IsWandering = false;
            _aiAgent.enabled = true;
            _roomTrigger.OnRoomEnter.RemoveListener(StopWander);
            
            // stop wander loop
            if (_wanderLoop != null) StopCoroutine(_wanderLoop);
            
            // clear wandering path
            _navAgent.ResetPath();
        }
        
        IEnumerator WanderLoop()
        {
            while (IsWandering)
            {
                // try to find wander destination
                if (!TrySampleWanderDestination(out var destination))
                {
                    Debug.LogWarning("AIAgent wandering was unable to find any destinations.");
                    StopWander();
                    yield break;
                }

                _navAgent.destination = destination;
                
                // wait till destination is reached
                yield return new WaitUntil(() => _navAgent.remainingDistance <= _navAgent.stoppingDistance);
                
                if (_delayBetweenWanders.Enabled)
                {
                    float wait = Random.Range(_delayBetweenWanders.Value.x, _delayBetweenWanders.Value.y);
                    yield return new WaitForSeconds(wait);
                }
            }
        }

        bool TrySampleWanderDestination(out Vector3 result)
        {
            float range = Mathf.Max(_navAgent.height * 2, _roomTrigger.Collider.bounds.size.y);
            result = new Vector3();
            for (int i = 0; i < MAX_WANDER_SAMPLES; i++)
            {
                Vector3 point = RandomPointInRoom();
                if (NavMesh.SamplePosition(point, out var hit, range, _navAgent.areaMask))
                {
                    result = hit.position;
                    return true;
                }
            }

            return false;
        }

        Vector3 RandomPointInRoom()
        {
            var bounds = _roomTrigger.Collider.bounds;
            Vector3 result = bounds.center;
            Vector3 roomExtents = bounds.extents;
            
            result.x = GetRandomInBounds1D(result.x, roomExtents.x);
            result.z = GetRandomInBounds1D(result.z, roomExtents.z);
            
            return result;

            float GetRandomInBounds1D(float center, float extents)
            {
                return Random.Range(center - extents, center + extents);
            }
        }

        #if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (IsWandering)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_navAgent.destination, 0.5f);
            }
        }
        #endif
    }
}