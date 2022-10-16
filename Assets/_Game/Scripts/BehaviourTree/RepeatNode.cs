using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BehaviourTree
{
    public class RepeatNode : DecoratorNode
    {
        protected override void OnStart()
        {
            
        }

        protected override void OnStop()
        {
            
        }

        protected override State OnUpdate()
        {
            child.Update();
            //Returns running regardless of child's success or failure
            return State.Running;
        }
    }
}
