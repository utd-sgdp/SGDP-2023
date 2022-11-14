using Game.Agent.Tree;
using UnityEngine;

namespace Game.Agent.Decorator
{
    public class RunOnce : DecoratorNode
    {
        bool _ran;
        
        protected override void OnStart() { Debug.Log("hello");}
        protected override void OnStop() => _ran = true;
        protected override State OnUpdate() => _ran ? State.Success : child.Update();
    }
}