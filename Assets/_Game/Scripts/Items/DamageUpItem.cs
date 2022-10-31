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
            PlayerWeapon pw = player.GetComponentInParent<PlayerWeapon>();
            if (pw == null) return;


            pw.Weapon.Multiplier += DamageIncrease;

            print("Damage increased by " + DamageIncrease * 100 + " percent");
        }
    }
}
