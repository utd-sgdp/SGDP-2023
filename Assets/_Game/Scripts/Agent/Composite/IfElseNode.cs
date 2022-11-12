using System;
using Codice.Client.BaseCommands.Differences;
using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;

namespace Game.Enemy.Composite
{
    public class IfElseNode : CompositeNode
    {
        protected override void OnStart()
        {
        }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            switch (Children[0].Update())
            {
                case State.Running:
                    return State.Running;
                case State.Success:
                    return Children[1].Update();
                default:
                    return Children[2].Update();
            }
        }
    }
}
