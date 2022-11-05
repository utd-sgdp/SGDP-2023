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
            Debug.Log($"{Vector3.Distance(Blackboard.transform.position, Blackboard.target.position)}");
            if ((Vector3.Distance(Blackboard.transform.position, Blackboard.target.position) > desiredDistanceMax)) {
                _agent.destination = Blackboard.target.position;
                return State.Running;
            }
            else if (Vector3.Distance(Blackboard.transform.position, Blackboard.target.position) <= desiredDistanceMin){
                var heading = Blackboard.target.position - GameObject.FindGameObjectWithTag("Player").transform.position;
                var direction = heading / heading.magnitude;
                //Probably should only randomize this once                
                _agent.destination = ((direction + new Vector3(Random.Range(-(direction.x*0.15f), (direction.x * 0.15f)), Random.Range(-(direction.y * 0.15f), (direction.y * 0.15f)), Random.Range(-(direction.z * 0.15f), (direction.z * 0.15f)))) * new Vector3(desiredDistanceMin, desiredDistanceMin, desiredDistanceMin).magnitude);
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
