using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;

namespace Game.Enemy.Action
{
    public class InAttackRangeNode : ActionNode
    {
        public float attackRange;

        protected override void OnStart() { }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            float distanceSQ = (Blackboard.target.position - Blackboard.transform.position).sqrMagnitude;
            return distanceSQ < attackRange * attackRange ? State.Success : State.Failure;
        }
    }
}