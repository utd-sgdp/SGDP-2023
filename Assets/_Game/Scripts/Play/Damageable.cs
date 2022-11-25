using UnityEngine;
using UnityEngine.Events;

namespace Game.Play
{
    public class Damageable : MonoBehaviour
    {
        public float Health => _health;
        public float MaxHealth => _maxHealth;
        public float HealthFraction => _health / _maxHealth;
        public bool AtMaxHealth => Mathf.Approximately(_health, _maxHealth);
        
        [Header("Data")]
        [SerializeField, ReadOnly]
        float _health = 100f;
        
        [SerializeField]
        [Min(0)]
        float _maxHealth = 100f;

        [Header("Events")]
        public UnityEvent<float> OnChange;
        public UnityEvent<float> OnHeal;
        public UnityEvent<float> OnHurt;
        public UnityEvent<float> OnMaxHeal;
        public UnityEvent<float> OnKill;

        #if UNITY_EDITOR
        void OnValidate()
        {
            _health = _maxHealth;
        }
        #endif

        [Button(Mode = ButtonMode.InPlayMode, Spacing = 25)]
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
        
        [Button(Mode = ButtonMode.InPlayMode)]
        public void Heal(float amount)
        {
            _health = Mathf.Min(_health + amount, _maxHealth);
            
            OnHeal?.Invoke(amount);
            OnChange?.Invoke(_health);
        }
        
        public void MaxHeal()
        {
            _health = _maxHealth;
            
            OnMaxHeal?.Invoke(_health);
            OnChange?.Invoke(_health);
        }
        
        public void Kill()
        {
            _health = 0;
            OnKill?.Invoke(_health);

            Destroy(gameObject);
        }

        public static Damageable Find(Collider collider)
        {
            return collider.attachedRigidbody
                       ? collider.attachedRigidbody.GetComponent<Damageable>()
                       : collider.GetComponent<Damageable>();
        }
    }
}

