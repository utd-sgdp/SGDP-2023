using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Weapons
{
    
    public class WeaponBomb : WeaponBase
    {
        
        [SerializeField] private float _explosionRadius = 5f;
        [SerializeField] private float _bombDamage = 50f;

        /// <summary>
        /// Attack method for any enemy with the bomb weapon. All game objects with the Damageable component will take the bomb damage
        /// </summary>
        public override void Attack()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius);

            //Deals damage by calling function from Damageable on everything in radius
            foreach (Collider hit in colliders)
            {                
                Damageable damaged = hit.gameObject.GetComponent<Damageable>();
                if (damaged != null) // make sure that the collided object has the damageable component before calling a method from that component.
                {
                    damaged.Hurt(_bombDamage);
                }                
            }
        }
    }
}