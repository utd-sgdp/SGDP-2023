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
        public bool moveAwayValid = true;
        
        protected override void OnStart()
        {
            s_agent.isStopped = false;
        }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            //Debug.Log($"{Vector3.Distance(Blackboard.transform.position, Blackboard.target.position)}");
            float distanceFromTargetSQ = (Blackboard.target.position - Blackboard.transform.position).sqrMagnitude;
            float desiredSQ = desiredDistanceMax * desiredDistanceMax; 
            if (distanceFromTargetSQ > desiredSQ)
            {
                s_agent.destination = Blackboard.target.position;
                s_agent.isStopped = false;
                moveAwayValid = true;
                return State.Running;
            }
            desiredSQ = desiredDistanceMin * desiredDistanceMin;
            if (s_agent.destination != null && ((Blackboard.transform.position - s_agent.destination).sqrMagnitude < s_agent.stoppingDistance * s_agent.stoppingDistance || (s_agent.destination - Blackboard.target.position).sqrMagnitude < desiredSQ))
            {
                moveAwayValid = true;
            }
            if (Blackboard.movementReference != null)
            {
                if (distanceFromTargetSQ <= desiredSQ && moveAwayValid)
                {
                    var direction = (Blackboard.target.position - Blackboard.movementReference.position).normalized;

                    // calculate movement away from target             
                    float angle = Random.Range(-moveAwayRandomRotation, moveAwayRandomRotation);
                    Vector3 nDest = Quaternion.Euler(0, angle, 0) * direction * (desiredDistanceMin + s_agent.stoppingDistance);
                    s_agent.destination = nDest;
                    s_agent.isStopped = false;
                    moveAwayValid = false;
                    return State.Running;
                }
            }
            else if (distanceFromTargetSQ <= desiredSQ && moveAwayValid)
            {
                var direction = (Blackboard.transform.position - Blackboard.target.position).normalized;
                
                // calculate movement away from target                
                float angle = Random.Range(-moveAwayRandomRotation, moveAwayRandomRotation);
                Vector3 nDest = Quaternion.Euler(0, angle, 0) * direction * (desiredDistanceMin + s_agent.stoppingDistance);
                s_agent.destination = nDest;
                s_agent.isStopped = false;
                moveAwayValid = false;
                return State.Running;
            }

            if (distanceFromTargetSQ > desiredDistanceMin * desiredDistanceMin)
            {
                s_agent.isStopped = true;
                moveAwayValid = true;
                return State.Success;
            }
            
            return State.Running;
        }
    }
}
