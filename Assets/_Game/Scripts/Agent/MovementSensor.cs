using System;
using Game.Animation;
using Game.Play.Items.Statistics;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Agent
{
    public class MovementSensor : MonoBehaviour, IMovementProvider, IStatTarget
    {
        #region Events
        public Action<Vector2> OnMove;
        public void SubscribeToOnMove(Action<Vector2> movement) => OnMove += movement;
        public void UnsubscribeToOnMove(Action<Vector2> movement) => OnMove -= movement;
        #endregion
        
        #if UNITY_EDITOR
        [SerializeField, ReadOnly]
        float _multiplier = 1;
        #endif
        
        NavMeshAgent _agent;
        float _baseMove;
        float _baseTurn;

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            
            _baseMove = _agent.speed;
            _baseTurn = _agent.angularSpeed;
        }
        
        void Update()
        {
            if (_agent.isStopped)
            {
                OnMove?.Invoke(Vector2.zero);
                return;
            }

            // calculate movement relative to the direction we are facing
            Vector3 velocity = _agent.velocity;
            velocity = Quaternion.Euler(0, -transform.rotation.eulerAngles.y, 0) * velocity;
            
            Vector2 move = new Vector2(velocity.x, velocity.z);
            OnMove?.Invoke(move);
        }

        public void OnStatChange(Stat stat)
        {
            #if UNITY_EDITOR
            _multiplier = stat.Value;
            #endif
            
            _agent.speed = _baseMove * stat.Value;
            _agent.angularSpeed = _baseTurn * stat.Value;
        }
    }
}