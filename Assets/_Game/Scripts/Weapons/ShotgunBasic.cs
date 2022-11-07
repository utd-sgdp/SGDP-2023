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
        [SerializeField]
        protected float _spreadAngle = 10f;
        
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

            Quaternion pellet;
            if (_hitScan)
            {
                RaycastHit hit;
                for(int i = 0; i < _buckshot; i++)
                {
                    pellet = UnityEngine.Random.rotation;
                    pellet = Quaternion.RotateTowards(_gunTip.transform.rotation, pellet, _spreadAngle);
                    if(Physics.Raycast(_gunTip.position, pellet*Vector3.forward, out hit, _range))
                    {
                        Debug.Log(hit.transform.name + " was hit at "+hit.point);
                    }
                }
                _bulletsLeft--;
                return;
            }

            for(int i = 0; i < _buckshot; i++)
            {
                pellet = UnityEngine.Random.rotation;
                pellet = Quaternion.RotateTowards(_gunTip.transform.rotation, pellet, _spreadAngle);
                _bulletPrefab.Spawn(_gunTip.position, pellet);
                _bulletsLeft--;
            }
        }

        IEnumerator reload()
        {
            _reloading = true;
            
            yield return new WaitForSeconds(_reloadTime);
            _reloading = false;
            
            if(_bulletsLeft < _magazineSize)
            {
                _bulletsLeft++;
            }
        }
    }
}
