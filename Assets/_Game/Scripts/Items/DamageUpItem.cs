using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using Game.Weapons;

namespace Game.Items
{
    public class DamageUpItem : ItemBase
    {
        [SerializeField]
        float DamageIncrease = .15f;

        public override void pickup(GameObject player)
        {
            ps.increaseDamageMultiplier(DamageIncrease);

            print("Damage increased by " + DamageIncrease * 100 + " percent");
        }
    }
}
