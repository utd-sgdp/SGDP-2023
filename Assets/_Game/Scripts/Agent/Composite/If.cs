using Game.Agent.Tree;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// Perform operation (second child) while condition is met (first child)
    /// </summary>
    public class If : CompositeNode
    {
        protected override void OnStart() { }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            State condition = Children[0].Update();
            
            // condition was met, perform operation
            if (condition == State.Success) return Children[1].Update();
            
            // propagate condition results
            return condition;
        }
    }
}
