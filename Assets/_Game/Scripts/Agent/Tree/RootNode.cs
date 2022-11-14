using System.Collections.Generic;
using UnityEngine;

namespace Game.Agent.Tree
{
    public sealed class RootNode : Node
    {
        [HideInInspector] public Node child;
        
        protected override void OnStart() { }
        protected override void OnStop() { }
        protected override State OnUpdate() => child.Update();

        public override List<Node> GetChildren()
        {
            var list = new List<Node>();
            if (child) list.Add(child);
            return list;
        }

        public override Tree.Node Clone()
        {
            RootNode node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}