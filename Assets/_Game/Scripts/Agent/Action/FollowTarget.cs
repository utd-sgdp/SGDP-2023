using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Agent.Action
{
    /// <summary>
    /// Follows <see cref="Blackboard.target"/> till in <see cref="desiredDistance"/>.
    /// </summary>
    public class FollowTarget : ActionNode
    {
        NavMeshAgent _agent
        {
            get
            {
                if (!b_agent)
                {
                    b_agent = GetComponent<NavMeshAgent>();
                }

                return b_agent;
            }
        }
        NavMeshAgent b_agent;
        
        public float desiredDistance; 

        protected override void OnStart()
        {
            // desiredDistance must be greater than or equal to stopping distance
            desiredDistance = Mathf.Max(desiredDistance, _agent.stoppingDistance);
            
            _agent.isStopped = false;
        }

        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            // wait till agent is in range of target
            bool inRange = SightSensor.InRange(Blackboard.transform, Blackboard.target, desiredDistance);
            
            if (!inRange)
            {
                _agent.destination = Blackboard.target.position;
            }
            
            return inRange ? State.Success : State.Running;
        }
        
        public override IReadOnlyCollection<string> GetDependencies() => new[] { typeof(NavMeshAgent).AssemblyQualifiedName };
    }
}
