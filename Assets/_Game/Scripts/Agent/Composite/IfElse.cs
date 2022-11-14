using System;
using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;

namespace Game.Agent.Composite
{
    /// <summary>
    /// Performs 1 of 2 operations based on the condition.
    /// </summary>
    public class IfElse : CompositeNode
    {
        protected override void OnStart() { }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            return Children[0].Update() switch
            {
                State.Running => State.Running,
                State.Success => Children[1].Update(),
                State.Failure => Children[2].Update(),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
