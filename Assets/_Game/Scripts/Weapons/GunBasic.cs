using System;
using System.Collections;
using System.Collections.Generic;
using Game.Utility;
using UnityEngine;

namespace Game.Weapons
{
    public class GunBasic : WeaponBase
    {
        public int MagazineSize => _magazineSize;
        public int BulletsLeft => _bulletsLeft;
        
        [Header("Stats")]
        [SerializeField]
        protected Optional<float> _spread = new();
        
        [SerializeField]
        protected int _magazineSize;
        
        [SerializeField]
        protected float _reloadTime;
        
        [SerializeField]
        protected bool _hitScan;
        
        [SerializeField, ReadOnly]
        protected int _bulletsLeft;

        [Header("References")]
        [SerializeField, HighlightIfNull]
        protected BulletBasic _bulletPrefab;
        
        [SerializeField, HighlightIfNull]
        protected Transform _gunTip;
        
        bool _reloading;

        protected override void Awake()
        {
            base.Awake();
            
            _bulletsLeft = _magazineSize;
        }
        
        protected override void OnAttack()
        {
            Fire();
            _bulletsLeft--;
        }

        public override bool CanAttack()
        {
            // propagate attack duration from WeaponBase
            if (!base.CanAttack()) return false;
            
            if (_reloading)
            {
                Debug.Log("The gun is still reloading...");
                return false;
            }

            if (_bulletsLeft <= 0)
            {
                StartCoroutine(reload());
                return false;
            }

            return true;
        }

        protected void Fire()
        {
            float spread = _spread.Enabled ? _spread.Value : 0;
            if (_hitScan)
            {
                bool hit = BulletBasic.HitScan(_gunTip.position, _gunTip.rotation, spread);
                return;
            }
            
            _bulletPrefab.Spawn(_gunTip.position, _gunTip.rotation, spread);
        }

        IEnumerator reload()
        {
            _reloading = true;
            
            yield return new WaitForSeconds(_reloadTime);
            _reloading = false;
            
            _bulletsLeft = _magazineSize;
        }
    }
}
