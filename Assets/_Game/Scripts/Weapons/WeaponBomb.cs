using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class WeaponBomb : WeaponBase
    {
        [SerializeField]
        float _explosionRadius = 5f;
        
        [SerializeField]
        float _bombDamage = 50f;

        /// <summary>
        /// Applys damage to all <see cref="Damageable"/>'s in range.
        /// </summary>
        public override void Attack()
        {
            foreach (var damageable in DamageablesInRange())
            {                
                damageable.Hurt(_bombDamage);
            }
        }

        /// <summary>
        /// Iterates through <see cref="Damageable"/> objects in range.
        /// <example> <code>
        /// foreach (Damageable damageable in DamageablesInRange())
        /// {
        ///     print(damageable);
        /// }
        /// </code></example>
        /// </summary>
        /// <returns></returns>
        IEnumerable<Damageable> DamageablesInRange()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);
            foreach (Collider hit in colliders)
            {
                Damageable damaged = hit.gameObject.GetComponent<Damageable>();
                if (damaged == null) continue;

                yield return damaged;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _explosionRadius);
        }
    }
}