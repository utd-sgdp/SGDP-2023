using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Damageable : MonoBehaviour
    {
        float _health;
        float _maxHealth;

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

