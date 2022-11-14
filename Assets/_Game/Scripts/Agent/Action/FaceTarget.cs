// This code based on code from Brackeys Enemy AI video (https://youtu.be/xppompv1DBg?t=420)

using Game.Agent.Tree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    /// <summary>
    /// Turns the <see cref="NavMeshAgent"/> to face <see cref="Blackboard.target"/> at a speed proportional
    /// to <see cref="NavMeshAgent.angularSpeed"/>.
    /// </summary>
    public class FaceTarget : ActionNode
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
        
        protected override void OnStart() { }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            Vector3 dirVector = (Blackboard.target.position - Blackboard.transform.position).normalized;
            float speed = _agent.angularSpeed * Time.deltaTime;
            Quaternion direction = Quaternion.LookRotation(new Vector3(dirVector.x, 0, dirVector.z));
            
            direction = Quaternion.RotateTowards(Blackboard.transform.rotation, direction, speed);
            Blackboard.transform.rotation = direction;

            return State.Success;
        }
        
        public override IReadOnlyCollection<string> GetDependencies() => new[] { typeof(NavMeshAgent).AssemblyQualifiedName };
    }
}
