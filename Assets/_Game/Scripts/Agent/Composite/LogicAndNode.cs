using System;
using Codice.Client.BaseCommands.Differences;
using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;

namespace Game.Enemy.Composite
{
    public class LogicAndNode : CompositeNode
    {
        protected override void OnStart() { }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            foreach (var child in Children)
            {
                if (child.Update() != State.Success) //Returns State.Failure if any child has failed or is running.
                {
                    return State.Failure;
                }
            }
            return State.Success;
        }
    }
}
