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
        public int magazineSize;
        public float reloadTime,fireRate;
        private bool reloading = false;
        public int bulletsLeft;

        [Header("References")]
        [SerializeField, HighlightIfNull]
        protected BulletBasic _bulletPrefab;
        
        [SerializeField, HighlightIfNull]
        protected Transform _gunTip;

        protected override void Awake() {
            bulletsLeft = magazineSize;
            ConfigureCollisions();
            DisableCollisions();
        }
        protected override void OnAttack()
        {
            if(reloading){
                Debug.Log("Reloading");
            }

            if(!reloading){
                _bulletPrefab.Spawn(_gunTip.position, _gunTip.rotation);
                bulletsLeft--;
            }
            
            if(bulletsLeft <= 0 && !reloading) {
                StartCoroutine(reload());
            }
        }

        IEnumerator reload()
        {
            reloading = true; 
            yield return new WaitForSeconds(reloadTime);
            bulletsLeft = magazineSize;
            reloading = false;
        }
    }
}
