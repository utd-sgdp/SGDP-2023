using System;
using Codice.Client.BaseCommands.Differences;
using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;

namespace Game.Enemy.Composite
{
    public class DoAllNode : CompositeNode
    {
        [SerializeField, ReadOnly]
        int _current;
        
        protected override void OnStart() => _current = 0;
        protected override void OnStop() { }

        protected override State OnUpdate()
        {
            Node child = Children[_current];
            child.Update();
            _current++;
            
            // If reached end of children list, return success, if not return running
            return _current == Children.Count ? State.Success : State.Running;
        }
    }
}
