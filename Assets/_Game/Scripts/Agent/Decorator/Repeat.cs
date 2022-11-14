using System;
using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using Game.Utility;
using UnityEngine;

namespace Game.Agent.Decorator
{
    public class Repeat : DecoratorNode
    {
        public Optional<int> LoopAmount = new();
        
        [SerializeField, ReadOnly]
        int _loopsDone;

        protected override void OnStart() => _loopsDone = 0;
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            State childState = child.Update();

            // ignore loop count
            if (!LoopAmount.Enabled)
            {
                return State.Running;
            }

            // update number of loops done
            if (childState is not State.Running)
            {
                _loopsDone++;
            }

            // exit if we have done enough loops
            return _loopsDone >= LoopAmount.Value ? State.Success : State.Running;
        }
    }
}
