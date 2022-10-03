using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class HealthSystem
    {
        private int maxHealth;
        private int currentHealth { get; set; }

        public HealthSystem(int _maxHealth)
        {
            this.maxHealth = _maxHealth;
            currentHealth = this.maxHealth;
        }
        public int GetCurrentHealth()
        {
            return currentHealth;
        }
        public void Damage(int amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                //Die();
            }
        }
        public void Heal(int amount)
        {
            currentHealth += amount;
            if (currentHealth >= maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }
}
