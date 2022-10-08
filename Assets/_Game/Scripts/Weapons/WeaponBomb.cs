using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Weapons
{
    
    public class WeaponBomb : WeaponBase
    {
        
        public float explosionRadius = 5f;
        public float bombDamage = 50f;
        public override void Attack()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            //Deals damage by calling function from Damageable on everything in radius
            foreach (Collider hit in colliders)
            {                
                Damageable damaged = hit.gameObject.GetComponent<Damageable>();
                if (damaged != null)
                {
                    damaged.Hurt(bombDamage);
                }                
            }
        }
    }
}