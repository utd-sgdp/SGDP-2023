using Game.Agent.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Weapons
{
    public class SuicideBomb : WeaponBomb
    {          
        /// <summary>
        /// Applies damage to all <see cref="Damageable"/>'s in range.
        /// </summary>
        protected override void OnAttack()
        {
            _explosionRadius = 3f;
            _bombDamage = 50f;
            foreach (var damageable in DamageablesInRange())
            {                
                damageable.Hurt(_bombDamage);
            }

            if (gameObject != null) {
                gameObject.GetComponentInParent<Damageable>().Hurt(100);
            }
        }

    }
}