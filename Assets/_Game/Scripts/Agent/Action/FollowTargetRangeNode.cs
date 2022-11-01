using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy.Action
{
    public class FollowTargetRangeNode : ActionNode
    {
        NavMeshAgent _agent;
        public float desiredDistanceMax;
        public float desiredDistanceMin;

        protected override void OnStart()
        {
            if (_agent == null)
            {
                _agent = Blackboard.gameObject.GetComponent<NavMeshAgent>();
            }
            _agent.isStopped = false;
            if (Blackboard.originalLocation.Equals(Vector3.zero))
            {
                Blackboard.originalLocation = Blackboard.gameObject.transform.position;
            }
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            //Debug.Log($"{Vector3.Distance(Blackboard.transform.position, Blackboard.target.position)}");
            
            if ((Vector3.Distance(Blackboard.transform.position, Blackboard.target.position) > desiredDistanceMax)) {
                _agent.destination = Blackboard.target.position;
                return State.Running;
            }
            else if (Vector3.Distance(Blackboard.transform.position, Blackboard.target.position) <= desiredDistanceMin){
                _agent.destination = Blackboard.originalLocation;
                //Debug.Log($"first one {_agent.destination}");
                return State.Running;
            }
            else
            {
                //Debug.Log($"second one {_agent.destination}");
                _agent.destination = Blackboard.transform.position;
                return State.Success;
            }
        }
    }
}
