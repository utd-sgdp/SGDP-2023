using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;

namespace Game
{
    [RequireComponent(typeof(PlayerWeapon))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerStats : MonoBehaviour
    {
        PlayerWeapon pw;
        PlayerMovement pm;
        // Start is called before the first frame update
        void Start()
        {
            pw = GetComponent<PlayerWeapon>();
            pm = GetComponent<PlayerMovement>();
        }

        public void increaseDamageMultiplier(float multiplier)
        {
            pw.Weapon.Multiplier += multiplier;
        }

        public void increaseSpeedMultiplier(float multiplier)
        {
            pm.Multiplier += multiplier;
        }
    }
}
