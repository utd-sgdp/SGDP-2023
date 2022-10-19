using System.Collections.Generic;

namespace Game.Agent.Tree
{
    public abstract class ActionNode : Node
    {
        public sealed override List<Node> GetChildren() => new();
    }
}
