using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;

namespace Game.Enemy.Action
{
    public class WaitNode : ActionNode
    {
        [Min(0)]
        public float _duration = 1f;
        
        [SerializeField, ReadOnly]
        float _elapsedTime;

        protected override void OnStart() => _elapsedTime = 0;
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            float elapsed = _elapsedTime;
            
            // update elapsed time (for the next frame)
            _elapsedTime += Time.deltaTime;
            
            return elapsed >= _duration ? State.Success : State.Running;
        }
    }
}
