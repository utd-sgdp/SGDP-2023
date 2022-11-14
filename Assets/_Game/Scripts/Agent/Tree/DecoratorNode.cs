using System.Collections.Generic;
using UnityEngine;

namespace Game.Agent.Tree
{
    public abstract class DecoratorNode : Node
    {
        public Node child;

        public sealed override List<Node> GetChildren()
        {
            var list = new List<Node>();
            if (child) list.Add(child);
            return list;
        }

        public override Node Clone()
        {
            DecoratorNode node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}
