// This code based on code from Brackeys Enemy AI video (https://youtu.be/xppompv1DBg?t=420)

using Game.Agent.Tree;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Agent.Action
{
    /// <summary>
    /// Turns the <see cref="NavMeshAgent"/> to face <see cref="Blackboard.target"/> at a speed proportional
    /// to <see cref="NavMeshAgent.angularSpeed"/>.
    /// </summary>
    public class FaceTarget : ActionNode
    {
        [SerializeField]
        [Min(0)]
        float _lookSpeed = 120f;
        
        [SerializeField]
        float _lookThreshold = 10f;

        float _normalizedSpeed => _lookSpeed * Time.deltaTime;
        
        protected override void OnStart() { }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            Vector3 dirVector = (Blackboard.target.position - transform.position).normalized;
            dirVector.y = 0;
            Quaternion direction = Quaternion.LookRotation(dirVector);
            
            direction = Quaternion.RotateTowards(transform.rotation, direction, _normalizedSpeed);
            float angle = Vector3.Angle(dirVector, transform.forward);

            transform.rotation = direction;
            return angle < _lookThreshold ? State.Success : State.Running;
        }
    }
}
