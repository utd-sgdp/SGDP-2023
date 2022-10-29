using System.Collections.Generic;

namespace Game.Agent.Tree
{
    public sealed class RootNode : Node
    {
        public Node child;
        
        protected override void OnStart() { }
        protected override void OnStop() { }
        protected override State OnUpdate() => child.Update();

        public override List<Node> GetChildren() => new() { child };

        public override Tree.Node Clone()
        {
            RootNode node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}