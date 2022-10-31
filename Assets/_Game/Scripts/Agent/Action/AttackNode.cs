using System.Collections;
using System.Collections.Generic;
using Game.Agent.Tree;
using Game.Weapons;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Enemy.Action
{
    public class AttackNode : ActionNode
    {
        WeaponHolder _weaponHolder;
        WeaponBase _weapon
        {
            get
            {
                if (!_weaponHolder)
                {
                    _weaponHolder = Blackboard.gameObject.GetComponent<WeaponHolder>();
                }

                if (!_weaponHolder) 
                {
                    Debug.LogError($"{Blackboard.gameObject} is missing a weapon holder.");
                }

                return _weaponHolder.Value;
            }
        }

        bool _isAttacking;

        protected override void OnStart()
        {
            _isAttacking = true;
            bool startedAttack = _weapon.AttemptAttack(AfterAttack: () =>
            {
                _isAttacking = false;
            });

            if (startedAttack)
            {
                _isAttacking = true;
            }
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return _isAttacking ? State.Running : State.Success;
        }
    }
}