using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy.Action
{
    public class ExplosionTelegraph : ActionNode
    {

        protected override void OnStart() { }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            float speed = Blackboard.gameObject.GetComponent<NavMeshAgent>().speed / 2;
            Blackboard.gameObject.GetComponent<NavMeshAgent>().speed = speed;
            return State.Success;
        }
    }
}
