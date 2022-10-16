using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.BehaviourTree
{
    public class BehaviourTreeTest : MonoBehaviour
    {
        BehaviourTree tree;
        // Start is called before the first frame update
        void Start()
        {
            //Create a tree and it's root node
            tree = ScriptableObject.CreateInstance<BehaviourTree>();

            var log1 = ScriptableObject.CreateInstance<DebugLogNode>();
            log1.message = "Hello 1";

            var log2 = ScriptableObject.CreateInstance<DebugLogNode>();
            log2.message = "Hello 2";

            var log3 = ScriptableObject.CreateInstance<DebugLogNode>();
            log3.message = "Hello 3";

            var wait1 = ScriptableObject.CreateInstance<WaitNode>();

            var wait2 = ScriptableObject.CreateInstance<WaitNode>();

            var wait3 = ScriptableObject.CreateInstance<WaitNode>();

            var repeat = ScriptableObject.CreateInstance<RepeatNode>();

            var sequencer = ScriptableObject.CreateInstance<SequencerNode>();
            //Set order to wait between each DebugLogNode
            sequencer.children.Add(log1);
            sequencer.children.Add(wait1);
            sequencer.children.Add(log2);
            sequencer.children.Add(wait2);
            sequencer.children.Add(log3);
            sequencer.children.Add(wait3);
            repeat.child = sequencer;

            tree.rootNode = repeat;
        }

        // Update is called once per frame
        void Update()
        {
            //Run the behaviour tree's update, not the root node's
            tree.Update();
        }
    }
}
