using System.Collections.Generic;

namespace Game.Agent.Tree
{
    public abstract class CompositeNode : Node
    {
        public List<Node> Children = new();

        public sealed override List<Node> GetChildren() => Children.Clone();

        public override Tree.Node Clone()
        {
            CompositeNode node = Instantiate(this);
            node.Children = Children.ConvertAll(child => child.Clone());
            return node;
        }
    }
}
