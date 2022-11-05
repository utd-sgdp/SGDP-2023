using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy.Action
{
    public class FollowTargetRangeNode : ActionNode
    {
        NavMeshAgent s_agent
        {
            get
            {
                if (!_agent)
                {
                    _agent = Blackboard.gameObject.GetComponent<NavMeshAgent>();
                }
                
                return _agent;
            }
        }
        NavMeshAgent _agent;
        
        public float desiredDistanceMax;
        public float desiredDistanceMin;
        public float moveAwayRandomRotation = 0.15f;
        
        protected override void OnStart()
        {
            if (Blackboard.originalLocation.Equals(Vector3.zero))
            {
                Blackboard.originalLocation = Blackboard.gameObject.transform.position;
            }
            
            s_agent.isStopped = false;
        }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            Debug.Log($"{Vector3.Distance(Blackboard.transform.position, Blackboard.target.position)}");

            float distanceFromTargetSQ = (Blackboard.target.position - Blackboard.transform.position).sqrMagnitude;
            float desiredSQ = desiredDistanceMax * desiredDistanceMax; 
            if (distanceFromTargetSQ > desiredSQ)
            {
                s_agent.destination = Blackboard.target.position;
                return State.Running;
            }
            
            desiredSQ = desiredDistanceMin * desiredDistanceMin; 
            if (distanceFromTargetSQ <= desiredSQ)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                
                var direction = (Blackboard.target.position - player.transform.position).normalized;
                
                // TODO: don't recalculate destination if still valid
                // calculate movement away from target                
                float angle = Random.Range(-moveAwayRandomRotation, moveAwayRandomRotation);
                Vector3 nDest = Quaternion.Euler(0, angle, 0) * direction * desiredDistanceMin;
                s_agent.destination = nDest;
                
                //Debug.Log($"first one {_agent.destination}");
                return State.Running;
            }
            
            //Debug.Log($"second one {_agent.destination}");
            s_agent.isStopped = false;
            return State.Success;
        }
    }
}
