using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;

namespace Game.Items
{
    public class SpeedUpItem : ItemBase
    {
        [SerializeField]
        float SpeedIncrease = .15f;

        public override void pickup(GameObject player)
        {
            PlayerMovement pm = player.GetComponentInParent<PlayerMovement>();
            if (pm == null) return;

            pm.Multiplier += SpeedIncrease;

            print("Speed increased by " + SpeedIncrease * 100 + " percent");
        }
    }
}
