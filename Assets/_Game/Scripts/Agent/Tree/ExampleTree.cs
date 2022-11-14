using Game.Agent.Tree;
using Game.Agent.Action;
using Game.Agent.Composite;
using Game.Agent.Decorator;
using UnityEngine;

namespace Game.Agent.Tree
{
    public sealed class ExampleTree : AIAgent
    {
        protected override BehaviourTree CreateTree()
        {
            // create tree
            var tree = ScriptableObject.CreateInstance<BehaviourTree>();
            
            // create nodes
            var repeat = ScriptableObject.CreateInstance<Repeat>();
            tree.RootNode = repeat;
            
            var sequencer = ScriptableObject.CreateInstance<Sequencer>();
            repeat.child = sequencer;
            
            var log1 = ScriptableObject.CreateInstance<DebugLog>();
            var log2 = ScriptableObject.CreateInstance<DebugLog>();
            var log3 = ScriptableObject.CreateInstance<DebugLog>();
            
            log1.Message = "Hello 1";
            log2.Message = "Hello 2";
            log3.Message = "Hello 3";
    
            var wait1 = ScriptableObject.CreateInstance<Wait>();
            var wait2 = ScriptableObject.CreateInstance<Wait>();
            var wait3 = ScriptableObject.CreateInstance<Wait>();
            
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