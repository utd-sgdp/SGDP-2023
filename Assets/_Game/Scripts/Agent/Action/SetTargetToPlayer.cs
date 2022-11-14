using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Agent.Tree;

namespace Game.Agent.Action
{
    /// <summary>
    /// Set <see cref="Blackboard.target"/> to the player.
    /// </summary>
    public class SetTargetToPlayer : ActionNode
    {
        bool _success;
        
        protected override void OnStart()
        {
            _success = true;
            GameObject _player = GameObject.Find("Player");
            
            // exit, unable to find player
            if (_player == null)
            {
                Debug.LogError($"{nameof(SetTargetToPlayer)} could not find an instance of the player.");
                _success = false;
                return;
            }

            Blackboard.target = _player.transform;
        }
        
        protected override void OnStop() { }
        protected override State OnUpdate() => _success ? State.Success : State.Failure;
    }
}
