using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Weapons
{
    [RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
    public class GunBasic : MonoBehaviour
    {
        //get input system and the prefab to be used for bullets (for now, before we have different, unique guns)
        PlayerInput _input;
        InputAction _fireAction;
        [SerializeField]
        BulletBasic _bulletPrefab;
        [SerializeField]
        Transform _gunTip;
        [SerializeField, Min(0)]
        [Tooltip("Bullets per second")]
        float _fireRate = 1;

        float _fireTimeStamp;

        public virtual void Fire()
        {
            //check if gun tip is null or the last shot was fired within the fire rate
            if (_gunTip == null) return;
            if (Time.time - _fireTimeStamp < _fireRate) return;
            _fireTimeStamp = Time.time;
            _bulletPrefab.Spawn(_gunTip.position, _gunTip.rotation);
        }
    }
}
