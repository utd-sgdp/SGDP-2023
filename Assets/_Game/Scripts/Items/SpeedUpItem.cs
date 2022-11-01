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
            ps.increaseSpeedMultiplier(SpeedIncrease);

            print("Speed increased by " + SpeedIncrease * 100 + " percent");
        }
    }
}
