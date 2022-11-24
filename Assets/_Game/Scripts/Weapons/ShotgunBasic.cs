using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class ShotgunBasic : GunBasic
    {
        [Header("Shotgun Stats")]
        [SerializeField]
        protected int _buckshot = 10;
        
        protected override void OnAttack()
        {
            for (int i = 0; i < _buckshot; i++)
            {
                Fire();
            }

            _bulletsLeft--;
        }
    }
}
