using System;
using System.Collections;
using System.Collections.Generic;
using Game.Play.Items.Statistics;
using Game.Play.Level;
using Game.Play;
using Game.Player;
using Game.Weapons;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerWeapon), typeof(PlayerMovement))]
    public class PlayerStats : MonoBehaviour
    {
        PlayerWeapon DamageTarget;
        public Stat Damage = new();
        
        IStatTarget SpeedTarget;
        public Stat Speed = new();
        
        public Damageable Damageable { get; private set; }
        
        void Awake()
        {
            DamageTarget = GetComponent<PlayerWeapon>();
            SpeedTarget = GetComponent<PlayerMovement>();
            Damageable = GetComponentInChildren<Damageable>();
        }

        // connect stat object to game behaviours
        void OnEnable()
        {
            Damage.OnChange += DamageTarget.Weapon.OnStatChange;
            Speed.OnChange += SpeedTarget.OnStatChange;
        }

        void OnDisable()
        {
            Damage.OnChange -= DamageTarget.Weapon.OnStatChange;
            Speed.OnChange  -= SpeedTarget.OnStatChange;
        }
    }
}
