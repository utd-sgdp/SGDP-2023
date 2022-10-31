using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class HealingItem : ItemBase
    {
        [SerializeField]
        float amountHealed = 50f;

        public override void pickup(GameObject player)
        {
            Damageable damageable = player.GetComponentInParent<Damageable>();
            if (damageable == null) return;

            damageable.Heal(amountHealed);

            print("Healed for " + amountHealed + " health");
        }
    }
}
