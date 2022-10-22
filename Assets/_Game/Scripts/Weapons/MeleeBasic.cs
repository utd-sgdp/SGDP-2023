using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class MeleeBasic : WeaponBase
    {
        public float damage = 10f;

        public bool IsAttacking = false;
        public GameObject Weapon;

        protected override void OnAttack()
        {
            //player is currently attacking, so check for collision in MeleeCollisionDetection
            IsAttacking = true;

            //play the attack animation
            Animator anim = Weapon.GetComponent<Animator>();
            anim.SetTrigger("Attack");
            attackDuration = anim.GetCurrentAnimatorClipInfo(0).Length;
        }
    }
}
