using System.Collections.Generic;

namespace Game.Agent.Tree
{
    public abstract class DecoratorNode : Node
    {
        public Node child;

        public sealed override List<Node> GetChildren() => new() { child };
    }
}
