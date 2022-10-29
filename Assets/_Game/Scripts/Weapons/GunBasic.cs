using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Weapons
{
    public class GunBasic : WeaponBase
    {
        [Header("Stats")]
        [Header("References")]
        [SerializeField, HighlightIfNull]
        protected BulletBasic _bulletPrefab;
        
        [SerializeField, HighlightIfNull]
        protected Transform _gunTip;

        protected override void OnAttack()
        {
            _bulletPrefab.Spawn(_gunTip.position, _gunTip.rotation);
        }
    }
}
