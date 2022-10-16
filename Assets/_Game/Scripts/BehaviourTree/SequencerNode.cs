using Codice.Client.BaseCommands.Differences;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BehaviourTree
{
    public class SequencerNode : CompositeNode
    {
        //Executes children in order. If one fails stops the node.
        int current;
        protected override void OnStart()
        {
            current = 0;
        }

        protected override void OnStop()
        {
           
        }

        protected override State OnUpdate()
        {
            var child = children[current];
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Success:
                    //If child succeeded, move to next child
                    current++;
                    break;
                case State.Failure:
                    return State.Failure;
            }
            //If reached end of children list, return success, if not return running
            return current == children.Count ? State.Success : State.Running;
        }
    }
}
