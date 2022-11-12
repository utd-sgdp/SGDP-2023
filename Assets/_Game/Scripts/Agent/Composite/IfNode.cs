using Game.Agent.Tree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class IfNode : CompositeNode
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            var childState = Children[0].Update();
            if (childState == State.Success)
            {
                return Children[1].Update();
            }
            else
            {
                return childState;
            }
        }
    }
}
