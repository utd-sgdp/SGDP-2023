using Game.Agent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Game.Agent.Tree;

namespace Game.Weapons
{
    public class OfficeHealerWeapon : WeaponBase
    {
        [Header("Stats")]
        [Header("References")]
        [SerializeField, HighlightIfNull]
        protected float healAmount;

        protected override void OnAttack()
        {
            attackDuration = 0.2f;
            gameObject.GetComponentInParent<AIAgent>()._tree.Blackboard.target.GetComponent<Damageable>().Heal(healAmount);
        }
    }
}
