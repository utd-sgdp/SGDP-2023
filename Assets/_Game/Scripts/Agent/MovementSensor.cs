using System;
using Game.Animation;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Agent
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementSensor : MonoBehaviour, IMovementProvider
    {
        #region Events
        public Action<Vector2> OnMove;
        public void SubscribeToOnMove(Action<Vector2> movement) => OnMove += movement;
        public void UnsubscribeToOnMove(Action<Vector2> movement) => OnMove -= movement;
        #endregion
        
        NavMeshAgent _agent;

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        
        void Update()
        {
            if (_agent.isStopped) return;

            // calculate movement relative to the direction we are facing
            Vector3 velocity = _agent.velocity;
            velocity = Quaternion.Euler(0, -transform.rotation.eulerAngles.y, 0) * velocity;
            
            Vector2 move = new Vector2(velocity.x, velocity.z);
            OnMove?.Invoke(move);
        }
    }
}