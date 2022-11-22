using System.Collections;
using System.Collections.Generic;
using Game.Play.Items.Statistics;
using UnityEngine;
using Game.Player;

namespace Game.Play.Items
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
