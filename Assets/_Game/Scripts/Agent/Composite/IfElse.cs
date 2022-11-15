using System;
using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;

namespace Game.Agent.Composite
{
    /// <summary>
    /// Performs 1 of 2 operations based on the condition.
    /// </summary>
    public class IfElse : CompositeNode
    {
        [SerializeField]
        bool _completeActionBeforeReevaluating;
        
        bool _evaluated;
        State _result;

        protected override void OnStart() => _result = Children[0].Update(); 
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            if (!_completeActionBeforeReevaluating)
            {
                _result = Children[0].Update();
            }
            
            // condition was met, perform operation
            if (_result == State.Success) return Children[1].Update();
            if (_result == State.Failure) return Children[2].Update();
            
            // propagate condition results
            return _result;
        }
    }
}
