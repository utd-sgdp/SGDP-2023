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
            _agent.isStopped = false;
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            _agent.destination = Blackboard.target.position;
            //return running until the agent gets close enough to the target
            return Vector3.Distance(Blackboard.transform.position, Blackboard.target.position) < desiredDistance ? State.Success : State.Running;
        }
    }
}
