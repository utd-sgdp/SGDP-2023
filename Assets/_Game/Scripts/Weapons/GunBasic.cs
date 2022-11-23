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
        
        [Header("Stats")]
        [SerializeField]
        int _magazineSize;
        
        [SerializeField]
        float _reloadTime;
        
        [SerializeField, ReadOnly]
        int _bulletsLeft;

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
            if (_reloading)
            {
                Debug.Log("The gun is still reloading...");
                return;
            }

            if (_bulletsLeft <= 0)
            {
                StartCoroutine(reload());
                return;
            }
            
            FireBullet();
        }

        public void FireBullet()
        {
            _bulletPrefab.Spawn(_gunTip.position, _gunTip.rotation);
            _bulletsLeft--;
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
