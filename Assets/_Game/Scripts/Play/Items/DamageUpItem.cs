using System.Collections;
using System.Collections.Generic;
using Game.Play.Items.Statistics;
using UnityEngine;
using Game.Player;
using Game.Weapons;

namespace Game.Play.Items
{
    public class DamageUpItem : ItemBase
    {
        [SerializeField]
        StatModifier _modifier;

        protected override void Pickup(PlayerStats player)
        {
            base.Pickup(player);
            player.Damage.AddModifier(_modifier);
        }
    }
}
