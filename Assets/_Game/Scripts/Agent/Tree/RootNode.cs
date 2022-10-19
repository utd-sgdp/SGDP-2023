using System.Collections.Generic;

namespace Game.Agent.Tree
{
    public sealed class RootNode : Node
    {
        public Node Child;
        
        protected override void OnStart() { }
        protected override void OnStop() { }
        protected override State OnUpdate() => Child.Update();

        public override List<Node> GetChildren() => new() { Child };
    }
}