using Game.Agent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Weapons
{
    public class OfficeHealerWeapon : WeaponBase
    {
        [Header("Stats")]
        [Header("References")]
        [SerializeField, HighlightIfNull]
        protected float healAmount = 2;

        protected override void OnAttack()
        {
            gameObject.GetComponentInParent<OfficeHealerAI>()._tree.Blackboard.target.GetComponent<Damageable>().Heal(healAmount);
        }
    }
}
