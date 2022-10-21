using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        [Header("Timing")]
        [SerializeField, Min(0)]
        protected float attackDuration;

        [SerializeField, Min(0)]
        protected float cooldownDuration;

        public bool Looping;

        [Header("Events")]
        public UnityEvent OnAttackStart;
        public UnityEvent OnAttackEnd;
        public UnityEvent OnCooldownStart;
        public UnityEvent OnCooldownEnd;

        protected bool attacking;
        protected bool coolingDown;

        public bool AttemptAttack(bool loop = false, System.Action AfterAttack = null, System.Action AfterCooldown = null)
        {
            Looping = loop;
            
            if (attacking || coolingDown)
            {
                return false;
            }

            Attack(AfterAttack, AfterCooldown);
            return true;
        }

        protected abstract void OnAttack();

        void Attack(System.Action AfterAttack, System.Action AfterCooldown)
        {
            OnAttack();
            StartCoroutine(AttackDelay(AfterAttack, AfterCooldown));
        }

        IEnumerator AttackDelay(System.Action AfterAttack, System.Action AfterCooldown)
        {
            // perform attack
            attacking = true;
            OnAttackStart?.Invoke();

            yield return new WaitForSeconds(attackDuration);
            AfterAttack?.Invoke();
            OnAttackEnd?.Invoke();

            // wait for cooldown
            attacking = false;
            coolingDown = true;
            OnCooldownStart?.Invoke();

            yield return new WaitForSeconds(cooldownDuration);
            AfterCooldown?.Invoke();
            coolingDown = false;
            OnCooldownEnd?.Invoke();
            
            // loop attack
            if (!Looping) yield break;
            AttemptAttack(true, AfterAttack, AfterCooldown);
        }
    }
}