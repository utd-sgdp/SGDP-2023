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

        [Header("Events")]
        public UnityEvent OnAttackStart;
        public UnityEvent OnAttackEnd;
        public UnityEvent OnCooldownStart;
        public UnityEvent OnCooldownEnd;

        protected bool attacking;
        protected bool coolingDown;

        public bool AttemptAttack(System.Action<DelayType> callback = null)
        {
            if (attacking || coolingDown)
            {
                return false;
            }

            Attack(callback);
            return true;
        }

        protected abstract void OnAttack();

        void Attack(System.Action<DelayType> callback)
        {
            OnAttack();
            StartCoroutine(AttackDelay(callback));
        }

        IEnumerator AttackDelay(System.Action<DelayType> callback)
        {
            // perform attack
            attacking = true;
            OnAttackStart?.Invoke();

            yield return new WaitForSeconds(attackDuration);
            callback?.Invoke(DelayType.AfterAttack);
            OnAttackEnd?.Invoke();

            // wait for cooldown
            attacking = false;
            coolingDown = true;
            OnCooldownStart?.Invoke();

            yield return new WaitForSeconds(cooldownDuration);
            callback?.Invoke(DelayType.AfterCooldown);
            coolingDown = false;
            OnCooldownEnd?.Invoke();
        }

        /// <summary>
        /// The current progress of an attack. I.e. <see cref="DelayType.AfterAttack"/> is after an attack is complete.
        /// </summary>
        public enum DelayType
        {
            AfterAttack = 0, AfterCooldown = 1,
        }
    }
}