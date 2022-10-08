using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Damageable : MonoBehaviour
    {
        [ReadOnly]
        [SerializeField] float _health = 100f;
        [SerializeField] float _maxHealth = 100f;

        public void Hurt(float amount)
        {
            _health -= amount;
            if(_health <= 0)
            {
                Kill();
            }
        }
        public void Heal(float amount)
        {
            _health += amount;
            if(_health > _maxHealth)
            {
                _health = _maxHealth;
            }
        }
        public void Kill()
        {

        }
    }
}

