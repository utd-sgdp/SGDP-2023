using System.Collections;
using System.Collections.Generic;
using Game.Items.Statistics;
using UnityEngine;
using Game.Player;

namespace Game.Items
{
    public class SpeedUpItem : ItemBase
    {
        [SerializeField]
        StatModifier _modifier;

        protected override void Pickup(PlayerStats player)
        {
            base.Pickup(player);
            player.Speed.AddModifier(_modifier);
        }
    }
}
