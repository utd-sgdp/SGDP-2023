using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy.Action
{
    public class FollowTargetNode : ActionNode
    {
        NavMeshAgent _agent;
        public float desiredDistance; 

        protected override void OnStart()
        {
            if (_agent == null)
            {
                _agent = Blackboard.gameObject.GetComponent<NavMeshAgent>();
            }

            //I don't desiredDistance should be set to less than the agent's stopping distance
            //If it is, the agent can get stuck and has weird behavior
            if (desiredDistance < _agent.stoppingDistance)
            {
                desiredDistance = _agent.stoppingDistance;
            }

            _agent.isStopped = false;
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            //return running until the agent gets close enough to the target
            if (Vector3.Distance(Blackboard.transform.position, Blackboard.target.position) > desiredDistance)
            {
                _agent.destination = Blackboard.target.position;
                return State.Running;
            }
            else
            {
                return State.Success;
            }
        }
    }
}
