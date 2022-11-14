using System;
using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;

namespace Game.Agent.Composite
{
    /// <summary>
    /// Runs <see cref="CompositeNode.Children"/> one-at-a-time, in order.
    /// </summary>
    public class Sequencer : CompositeNode
    {
        [SerializeField, ReadOnly]
        int _current;
        
        protected override void OnStart() => _current = 0;
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            Node child = Children[_current];
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                
                case State.Success:
                    // this child is finished
                    // move to next child
                    _current++;
                    break;
                
                default:
                case State.Failure:
                    return State.Failure;
            }
            
            // If reached end of children list, return success, if not return running
            return _current == Children.Count ? State.Success : State.Running;
        }
    }
}
