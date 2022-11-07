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
                return State.Running;
            }
            desiredSQ = desiredDistanceMin * desiredDistanceMin;
            if (Blackboard.movementReference != null)
            {
                if (distanceFromTargetSQ <= desiredSQ)
                {
                    var direction = (Blackboard.target.position - Blackboard.movementReference.position).normalized;

                    // TODO: don't recalculate destination if still valid
                    // calculate movement away from target                
                    float angle = Random.Range(-moveAwayRandomRotation, moveAwayRandomRotation);
                    Vector3 nDest = Quaternion.Euler(0, angle, 0) * direction * (desiredDistanceMin + s_agent.stoppingDistance);
                    s_agent.destination = nDest;
                    s_agent.isStopped = false;
                    return State.Running;
                }
            }
            if (distanceFromTargetSQ <= desiredSQ)
            {
                var direction = (Blackboard.transform.position - Blackboard.target.position).normalized;

                // TODO: don't recalculate destination if still valid
                // calculate movement away from target                
                float angle = Random.Range(-moveAwayRandomRotation, moveAwayRandomRotation);
                Vector3 nDest = Quaternion.Euler(0, angle, 0) * direction * (desiredDistanceMin + s_agent.stoppingDistance);
                s_agent.destination = nDest;
                s_agent.isStopped = false;
                return State.Running;
            }

            s_agent.isStopped = true;
            return State.Success;
        }
    }
}
