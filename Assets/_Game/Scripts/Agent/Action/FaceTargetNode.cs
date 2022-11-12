using Game.Agent.Tree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class FaceTargetNode : ActionNode
    {

        NavMeshAgent _agent;
        protected override void OnStart()
        {
            if (_agent == null)
            {
                _agent = Blackboard.gameObject.GetComponent<NavMeshAgent>();
            }
        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            //This code based on code from Brackeys Enemy AI video (https://youtu.be/xppompv1DBg?t=420)
            Vector3 direction = (Blackboard.target.position - Blackboard.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            Blackboard.transform.rotation = Quaternion.RotateTowards(Blackboard.transform.rotation, lookRotation, _agent.angularSpeed * Time.deltaTime);

            return State.Success;
        }

    }
}