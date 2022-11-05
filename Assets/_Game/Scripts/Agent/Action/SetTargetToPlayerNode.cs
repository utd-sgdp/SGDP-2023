using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Agent.Tree;

namespace Game.Enemy.Action
{
    public class SetTargetToPlayerNode : ActionNode
    {
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override State OnUpdate()
        {
            GameObject _player = GameObject.FindGameObjectWithTag("Player"); ; //Find the player object
            if (_player == null) //Check to see that we found it
            {
                Debug.LogError("SetTargetToPlayerNode could not find Game Object \"Player\"");
                return State.Failure;
            }

            Transform _transform = _player.transform;
            if (_transform == null)
            {
                Debug.LogError("SetTargetToPlayerNode could not find a transform component");
                return State.Failure;
            }
            Blackboard.target = _transform; //Set the transform
            return State.Success;
        }

    }
}
