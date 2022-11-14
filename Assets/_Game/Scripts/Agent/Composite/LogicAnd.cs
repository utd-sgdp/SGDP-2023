using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Agent.Tree;

namespace Game.Agent.Composite
{
    /// <summary>
    /// Returns <see cref="State.Success"/>, if all children's update returns <see cref="State.Success"/>. Else returns
    /// <see cref="State.Failure"/>
    /// </summary>
    public class LogicAnd : CompositeNode
    {
        protected override void OnStart() { }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            if (Children.Any(child => child.Update() != State.Success))
            {
                return State.Failure;
            }

            return State.Success;
        }
    }
}
