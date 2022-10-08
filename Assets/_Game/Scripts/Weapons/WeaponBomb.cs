using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Weapons
{
    
    public class WeaponBomb : WeaponBase
    {
        
        public float explosionRadius = 5f;
        public float bombDamage = 25f;
        public override void Attack()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            
           
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