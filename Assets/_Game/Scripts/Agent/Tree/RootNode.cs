using System.Collections.Generic;
using Game.Utility;
using UnityEngine;

namespace Game.Agent.Tree
{
    public sealed class RootNode : Node
    {
        public Node child;
        
        public Optional<int> LoopAmount = new();
        
        [SerializeField, ReadOnly]
        int _loopsDone;

        protected override void OnStart() => _loopsDone = 0;
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            State childState = child.Update();

            // ignore loop count
            if (!LoopAmount.Enabled)
            {
                return State.Running;
            }

            // update number of loops done
            if (childState is not State.Running)
            {
                _loopsDone++;
            }

            // exit if we have done enough loops
            return _loopsDone >= LoopAmount.Value ? State.Success : State.Running;
        }

        public override List<Node> GetChildren()
        {
            var list = new List<Node>();
            if (child) list.Add(child);
            return list;
        }

        public override Node Clone()
        {
            RootNode node = Instantiate(this);
            node.child = child.Clone();
            return node;
        }
    }
}