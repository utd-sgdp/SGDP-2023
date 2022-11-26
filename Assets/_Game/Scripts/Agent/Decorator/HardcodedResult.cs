using System;
using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using UnityEngine;

namespace Game.Agent.Decorator
{
    public class HardcodedResult : DecoratorNode
    {
        [SerializeField]
        State _state;

        #if UNITY_EDITOR
        void OnValidate()
        {
            Description = _state.ToString();
        }
        #endif

        protected override void OnStart() { }
        protected override void OnStop() { }
        
        protected override State OnUpdate()
        {
            child.Update();
            return _state;
        }
    }
}
