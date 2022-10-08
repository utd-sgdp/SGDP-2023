using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Damageable : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField] float _health = 100f;
        [SerializeField] float _maxHealth = 100f;

        public UnityEvent<float> onHeal;
        public UnityEvent<float> onHurt;
        public UnityEvent onMaxHeal;
        public UnityEvent onKill;
        public UnityEvent onChange;


        public void Hurt(float amount)
        {
            onHurt?.Invoke(amount);
            onChange?.Invoke();

            _health -= amount;
            if(_health <= 0) Kill();
        }
        public void Heal(float amount)
        {
            onHeal?.Invoke(amount);
            onChange?.Invoke();

            _health = Mathf.Min(_health + amount, _maxHealth);
        }
        public void MaxHeal()
        {
            onMaxHeal?.Invoke();
            onChange?.Invoke();

            _health = _maxHealth;
        }
        public void Kill()
        {
            onKill?.Invoke();

            Destroy(gameObject);
        }
    }
}

