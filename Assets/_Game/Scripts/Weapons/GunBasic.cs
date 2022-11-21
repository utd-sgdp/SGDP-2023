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

       protected enum reloadMode 
        {
            magazineReload,
            incrementReload,
        }
        
        [Header("Stats")]
        [SerializeField]
        protected Optional<float> _spread = new();
        
        [SerializeField]
        protected int _magazineSize;
        
        [SerializeField]
        protected float _reloadTime;
        
        [SerializeField]
        protected bool _hitScan;

        [SerializeField]
        protected reloadMode _reloadMode;
        
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
            
            // reload cancel
            if (_reloading)
            {
                Debug.Log("The gun is still reloading...");
                return false;
            }

            // start reloading
            if (_bulletsLeft <= 0)
            {
                Reload();
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
                
                // TODO: apply damage to whatever was hit
                
                return;
            }
            
            _bulletPrefab.Spawn(_gunTip.position, _gunTip.rotation, spread);
        }

        /// <summary>
        /// Reload method encapsulating the separate implementations of the reload functions
        /// </summary>
        void Reload()
        {
            switch (_reloadMode)
            {
                default:
                case reloadMode.magazineReload:
                    StartCoroutine(magazineReload());
                    break;
                
                case reloadMode.incrementReload:
                    StartCoroutine(incrementReload());
                    break;
            }
        }

        IEnumerator magazineReload()
        {
            _reloading = true;

            yield return new WaitForSeconds(_reloadTime);

            // reload was cancelled
            if (!_reloading) yield break;
            
            _reloading = false;
            _bulletsLeft = _magazineSize;
        }

        IEnumerator incrementReload() 
        {
            _reloading = true;
            while (_bulletsLeft < _magazineSize && _reloading)
            {
                yield return new WaitForSeconds(_reloadTime / _magazineSize);
                
                // reload was cancelled
                if (!_reloading) yield break;
                
                // continue reload
                _bulletsLeft++;
            }
            
            _reloading = false;
        }
    }
}
