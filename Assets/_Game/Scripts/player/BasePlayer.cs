using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class BasePlayer : MonoBehaviour
    {
        private HealthSystem health;
        public BasePlayer()
        {
            health = new HealthSystem(200);
        }

        public int GetHealth()
        {
            return health.GetCurrentHealth();
        }
        public virtual void Damage(int amount)
        {
            health.Damage(amount);
            if(health.GetCurrentHealth() <= 0)
            {
                Die();
            }
        }
        public virtual void Heal(int amount)
        {
            health.Heal(amount);
        }
        public virtual void Die()
        {

        }
    }
}
