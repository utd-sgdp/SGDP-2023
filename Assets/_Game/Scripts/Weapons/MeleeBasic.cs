using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [RequireComponent(typeof(Animator))]
    public class MeleeBasic : WeaponBase
    {
        Animator _anim;
        Animator _animator
        {
            get
            {
                if (!_anim)
                {
                    _anim = GetComponent<Animator>();
                }

                return _anim;
            }
        }

        protected override void OnAttack()
        {
            // play the attack animation
            _animator.SetTrigger(ST_ATTACK);
            
            // set attack length
            AnimationClip currentClip = _anim.GetCurrentAnimatorClipInfo(0)[0].clip;
            attackDuration = currentClip.length;
        }
        
        static readonly int ST_ATTACK = Animator.StringToHash("Attack");
    }
}
