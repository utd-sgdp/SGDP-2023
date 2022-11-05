using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public class HealingItem : ItemBase
    {
        [SerializeField]
        float amountHealed = 50f;

        protected override void Pickup(PlayerStats player)
        {
            base.Pickup(player);
            player.Damageable.Heal(amountHealed);
        }
    }
}
