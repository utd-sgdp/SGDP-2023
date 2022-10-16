using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BehaviourTree
{
    public abstract class DecoratorNode : Node
    {
        public Node child;
    }
}
