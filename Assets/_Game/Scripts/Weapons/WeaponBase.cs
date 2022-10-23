using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Weapons
{
    public abstract class WeaponBase : MonoBehaviour
    {
        public float Damage => damage;
        
        [Header("Damage")]
        [SerializeField]
        protected float damage = 10f;
        
        [Header("Timing")]
        [SerializeField, Min(0)]
        protected float attackDuration;

        [SerializeField, Min(0)]
        protected float cooldownDuration;

        public bool Looping;
        
        [Header("Targeting")]
        public LayerMask targetLayer;

        [Header("Events")]
        public UnityEvent OnAttackStart;
        public UnityEvent OnAttackEnd;
        public UnityEvent OnCooldownStart;
        public UnityEvent OnCooldownEnd;

        protected bool attacking;
        protected bool coolingDown;
        WeaponCollision[] _collidables;

        protected virtual void Awake()
        {
            ConfigureCollisions();
            DisableCollisions();
        }

        public void ConfigureCollisions()
        {
            _collidables = GetComponentsInChildren<WeaponCollision>();
            foreach (var col in _collidables)
            {
                col.Configure(this, targetLayer);
            }
        }

        /// <summary>
        /// Enables all weapon collisions.
        /// Can be called by <see cref="AnimationEvent"/>s.
        /// </summary>
        public void EnableCollisions()
        {
            foreach (var col in _collidables)
            {
                col.Enable();
            }
        }

        /// <summary>
        /// Disables all weapon collisions.
        /// Can be called by <see cref="AnimationEvent"/>s.
        /// </summary>
        public void DisableCollisions()
        {
            foreach (var col in _collidables)
            {
                col.Disable();
            }
        }

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