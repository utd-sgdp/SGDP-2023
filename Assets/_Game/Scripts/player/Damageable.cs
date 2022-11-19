using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Damageable : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField, ReadOnly]
        public float _health = 100f;
        
        [SerializeField]
        [Min(0)]
        public float _maxHealth = 100f;

        [Header("Events")]
        public UnityEvent<float> OnChange;
        public UnityEvent<float> OnHeal;
        public UnityEvent<float> OnHurt;
        public UnityEvent<float> OnMaxHeal;
        public UnityEvent<float> OnKill;

        public void Hurt(float amount)
        {
            _health = Mathf.Max(_health - amount, 0);

            if (_health == 0)
            {
                Kill();
            }
            
            OnHurt?.Invoke(_health);
            OnChange?.Invoke(_health);
        }
        
        public void Heal(float amount)
        {
            OnHeal?.Invoke(amount);
            OnChange?.Invoke(_health);

            _health = Mathf.Min(_health + amount, _maxHealth);
        }
        
        public void MaxHeal()
        {
            OnMaxHeal?.Invoke(_health);
            OnChange?.Invoke(_health);

            _health = _maxHealth;
        }
        
        public void Kill()
        {
            OnKill?.Invoke(_health);

            Destroy(gameObject);
        }
    }
}

