using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Agent.Action
{
    public class KeepDistanceFromTarget : ActionNode
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
        Vector2 _range;
        
        [SerializeField]
        float moveAwayRotation = 30f;
        
        [SerializeField]
        float moveAwayDistance = 5f;

        protected override void OnStart() => _agent.isStopped = false;
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            // exit, target was destroyed
            if (!Blackboard.target)
            {
                _agent.isStopped = true;
                return State.Failure;
            }
            
            bool tooClose = SightSensor.InRange(Blackboard.target, transform, _range.x); 
            bool tooFar  = !SightSensor.InRange(Blackboard.target, transform, _range.y);
            MoveDirection result = (tooClose, tooFar) switch
            {
                (false, false) => MoveDirection.MaintainDistance,
                (true, false) => MoveDirection.AwayFromTarget,
                (false, true) => MoveDirection.TowardsTarget,
                (true, true) => throw new System.ArgumentOutOfRangeException(),
            };

            switch (result)
            {
                case MoveDirection.TowardsTarget:
                    _agent.destination = Blackboard.target.position;
                    return State.Running;
                
                case MoveDirection.MaintainDistance:
                    _agent.isStopped = true;
                    return State.Success;
                
                case MoveDirection.AwayFromTarget:
                    Vector3 position = transform.position;
                    Vector3 direction = (position - Blackboard.target.position).normalized;
                    
                    // randomize position away from target
                    float angle = Random.Range(-moveAwayRotation, moveAwayRotation);
                    float distance = _range.x * Random.Range(0, moveAwayDistance);
                    direction = Quaternion.Euler(0, angle, 0) * direction * distance;
                    
                    _agent.destination = direction + position;
                    return State.Success;
                
                default:
                    throw new System.ArgumentOutOfRangeException();
            }
        }

        enum MoveDirection
        {
            TowardsTarget = 0, MaintainDistance = 1, AwayFromTarget = 2,
        }
        
        public override IReadOnlyCollection<string> GetDependencies() => new[] { typeof(NavMeshAgent).AssemblyQualifiedName };
    }
}