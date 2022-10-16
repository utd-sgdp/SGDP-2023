using Codice.Client.Common.TreeGrouper;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace Game.BehaviourTree
{
    [CreateAssetMenu()]
    public class BehaviourTree : ScriptableObject
    {
        public Node rootNode;
        public Node.State treeState = Node.State.Running;

        public Node.State Update()
        {
            if (rootNode.state == Node.State.Running)
            {
                treeState = rootNode.Update();
            }
            

            return treeState;
        }
    }
}
