using System;
using System.Collections;
using System.Collections.Generic;
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
            incrementReload
        }
        
        [Header("Stats")]
        [SerializeField]
        protected int _magazineSize;
        [SerializeField]
        protected float _range = 100f;

        [SerializeField]
        protected float _reloadTime;
        [SerializeField]
        protected bool _hitScan = false;

        [SerializeField]
        protected reloadMode _reloadMode;
        
        [SerializeField, ReadOnly]
        protected int _bulletsLeft;

        [Header("References")]
        [SerializeField, HighlightIfNull]
        protected BulletBasic _bulletPrefab;
        
        [SerializeField, HighlightIfNull]
        protected Transform _gunTip;
        
        protected bool _reloading;

        protected override void Awake()
        {
            base.Awake();
            
            _bulletsLeft = _magazineSize;
        }
        
        protected override void OnAttack()
        {
            if (_reloading)
            {
                if (_reloadMode == reloadMode.incrementReload)
                {
                    Debug.Log("Reload interrupted!");
                    _reloading = false;
                }
                else if(_reloadMode == reloadMode.magazineReload) 
                {
                    Debug.Log("Currently reloading...");
                    return;
                }
            }

            if (_bulletsLeft <= 0)
            {
                reload();
                return;
            }

            if (_hitScan)
            {
                RaycastHit hit;
                if(Physics.Raycast(_gunTip.position, _gunTip.forward, out hit, _range))
                {
                    Debug.Log(hit.transform.name+" was hit at "+hit.point);
                }
                _bulletsLeft--;
                return;
            }

            _bulletPrefab.Spawn(_gunTip.position, _gunTip.rotation);
            _bulletsLeft--;
        }

        /// <summary>
        /// Reload method encapsulating the separate implementations of the reload functions
        /// </summary>
        void reload()
        {
            if(_reloadMode == reloadMode.magazineReload)
            {
               StartCoroutine(magazineReload());
            }
            else if(_reloadMode == reloadMode.incrementReload) 
            {
               StartCoroutine(incrementReload());
            }
        }

        private IEnumerator magazineReload()
        {
            _reloading = true;

            yield return new WaitForSeconds(_reloadTime);
            _reloading = false;

            _bulletsLeft = _magazineSize;
        }

        private IEnumerator incrementReload() 
        {
            _reloading = true;
            while(_bulletsLeft < _magazineSize && _reloading) {
                yield return new WaitForSeconds(_reloadTime / _magazineSize);
                // Ensure that we are still reloading before we load a bullet
                if (_reloading) 
                {
                    _bulletsLeft++;
                    Debug.Log("Bullet loaded: " + _bulletsLeft + "/" + _magazineSize);
                }
            }
            _reloading = false;
        }
    }
}
