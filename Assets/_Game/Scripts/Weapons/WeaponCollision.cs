using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [RequireComponent(typeof(MeleeBasic))]
    public class WeaponCollision : MonoBehaviour
    {
        MeleeBasic mb;



        public void Awake()
        {
            mb = this.gameObject.GetComponent<MeleeBasic>();
        }
        private void OnTriggerEnter(Collider other)
        {
            //if the player was attacking and the weapon hit an enemy
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && mb.getAttacking())
            {
                //deal damage
                other.gameObject.GetComponent<Damageable>().Hurt(mb.damage);
            }
        }
    }
}
