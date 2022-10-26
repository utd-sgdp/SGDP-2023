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
        bool conditionEvaluated;
        bool conditionEvaluatedTo;

        protected override void OnStart()
        {
            conditionEvaluated = false;
        }
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            if (!conditionEvaluated)
            {
                switch (Children[0].Update())
                {
                    case State.Running:
                        return State.Running;

                    case State.Success:
                        conditionEvaluated = true;
                        conditionEvaluatedTo = true;
                        break;

                    case State.Failure:
                        conditionEvaluated = true;
                        conditionEvaluatedTo = false;
                        break;
                }
            }

            return conditionEvaluatedTo ? Children[1].Update() : Children[2].Update();
        }
    }
}
