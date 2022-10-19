using Game.Agent.Tree;
using Game.Enemy.Action;
using Game.Enemy.Composite;
using Game.Enemy.Decorator;
using UnityEngine;

namespace Game.Agent
{
    public sealed class ExampleTree : BehaviourTreeRunner
    {
        protected override BehaviourTree CreateTree()
        {
            // create tree
            var tree = ScriptableObject.CreateInstance<BehaviourTree>();
            
            // create nodes
            var repeat = ScriptableObject.CreateInstance<RepeatNode>();
            tree.RootNode = repeat;
            
            var sequencer = ScriptableObject.CreateInstance<SequencerNode>();
            repeat.child = sequencer;
            
            var log1 = ScriptableObject.CreateInstance<DebugLogNode>();
            var log2 = ScriptableObject.CreateInstance<DebugLogNode>();
            var log3 = ScriptableObject.CreateInstance<DebugLogNode>();
            
            log1.Message = "Hello 1";
            log2.Message = "Hello 2";
            log3.Message = "Hello 3";
    
            var wait1 = ScriptableObject.CreateInstance<WaitNode>();
            var wait2 = ScriptableObject.CreateInstance<WaitNode>();
            var wait3 = ScriptableObject.CreateInstance<WaitNode>();
            
            // sequencer - connect children
            sequencer.Children.Add(log1);
            sequencer.Children.Add(wait1);
            sequencer.Children.Add(log2);
            sequencer.Children.Add(wait2);
            sequencer.Children.Add(log3);
            sequencer.Children.Add(wait3);
            
            return tree;
        }
    }
}