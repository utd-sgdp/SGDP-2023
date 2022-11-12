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
            float _distance = Vector3.Distance(Blackboard.transform.position, Blackboard.target.position);
            return _distance < attackRange ? State.Success : State.Failure;
        }
    }
}