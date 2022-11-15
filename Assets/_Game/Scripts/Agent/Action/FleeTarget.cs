using System.Collections.Generic;
using Game.Agent;
using Game.Agent.Tree;
using UnityEngine;
using UnityEngine.AI;

namespace Game._Game.Scripts.Agent.Action
{
    public class FleeTarget : ActionNode
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

        [SerializeField]
        float _interval = 10;
        
        protected override void OnStart() => _agent.isStopped = false;
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            if (!Blackboard.target)
            {
                _agent.isStopped = true;
                return State.Failure;
            }
            
            // wait till agent is out of range of target
            bool inRange = SightSensor.InRange(Blackboard.transform, Blackboard.target, _interval);
            if (!inRange) return State.Success;

            // move away
            var position = transform.position;
            Vector3 direction = position - Blackboard.target.position;
            direction = direction.normalized * _interval;
            
            _agent.destination = direction + position;
            return State.Running;
        }
        
        public override IReadOnlyCollection<string> GetDependencies() => new[] { typeof(NavMeshAgent).AssemblyQualifiedName };
    }
}