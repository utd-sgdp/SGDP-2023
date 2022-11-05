using Game.Agent.Action;
using Game.Agent.Tree;
using Game.Enemy.Action;
using Game.Enemy.Composite;
using Game.Enemy.Decorator;
using UnityEngine;

namespace Game.Agent
{
    public sealed class OfficeHealerAI : AIAgent
    {
        protected override BehaviourTree CreateTree()
        {
            
            // create tree
            var tree = ScriptableObject.CreateInstance<BehaviourTree>();

            // create nodes
            var rootNode = ScriptableObject.CreateInstance<RootNode>();
            tree.RootNode = rootNode;

            var repeat = ScriptableObject.CreateInstance<RepeatNode>();
            rootNode.child = repeat;
            
            
            
            var ifFoundEnemy = ScriptableObject.CreateInstance<IfNode>();
            var findDamagedEnemy = ScriptableObject.CreateInstance<FindDamagedEnemy>();
            var repeatBody = ScriptableObject.CreateInstance<RepeatNode>();
            ifFoundEnemy.Children.Add(findDamagedEnemy);
            ifFoundEnemy.Children.Add(repeatBody);
            repeat.child = ifFoundEnemy;

            var sequencer = ScriptableObject.CreateInstance<SequencerNode>();
            repeatBody.child = sequencer;


            var ifInRange = ScriptableObject.CreateInstance<IfNode>();           
            var ifCanSee = ScriptableObject.CreateInstance<IfNode>();
            var inAttackRange = ScriptableObject.CreateInstance<InAttackRangeNode>();
            inAttackRange.attackRange = 6;
            ifInRange.Children.Add(inAttackRange);
            ifInRange.Children.Add(ifCanSee);
            var inLineOfSight = ScriptableObject.CreateInstance<InLineOfSightNodeHealer>();
            inLineOfSight.range = 100;
            var attack = ScriptableObject.CreateInstance<AttackNode>();
            ifCanSee.Children.Add(inLineOfSight);
            ifCanSee.Children.Add(attack);
            
            var follow = ScriptableObject.CreateInstance<FollowTargetRangeNode>();
            follow.desiredDistanceMax = 5;
            follow.desiredDistanceMin = 4;
            sequencer.Children.Add(follow);
            sequencer.Children.Add(ifInRange);
            //Check if new enemies are hurt
            sequencer.Children.Add(findDamagedEnemy);



            return tree;
        }
    }
}
