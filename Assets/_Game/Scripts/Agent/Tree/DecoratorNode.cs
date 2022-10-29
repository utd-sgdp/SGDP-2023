using System.Collections.Generic;
using UnityEngine;

namespace Game.Agent.Tree
{
    public abstract class DecoratorNode : Node
    {
        public Node child;

        public sealed override List<Node> GetChildren() => new() { child };

        public override Tree.Node Clone()
        {
            DecoratorNode node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}
