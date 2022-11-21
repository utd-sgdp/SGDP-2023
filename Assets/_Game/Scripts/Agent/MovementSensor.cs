using System;
using Game.Play.Items.Statistics;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Agent
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MovementSensor : MonoBehaviour, IStatTarget
    {
        #if UNITY_EDITOR
        [SerializeField, ReadOnly]
        float _multiplier = 1;
        #endif
        
        NavMeshAgent _agent;
        float _baseMove = 3.5f;
        float _baseTurn = 120f;

        void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _baseMove = _agent.speed;
            _baseTurn = _agent.angularSpeed;
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