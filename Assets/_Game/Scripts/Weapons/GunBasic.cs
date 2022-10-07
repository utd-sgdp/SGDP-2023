using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Weapons
{
    public class GunBasic : MonoBehaviour
    {
        /// <summary>
        /// if true, the gun will fire on a loop as fast as possible.
        /// </summary>
        public bool Firing
        {
            get => _firing;
            set
            {
                if (_firing == value) return;

                _firing = value;
                SetFiringLoop();
            }
        }
        
        [Header("Stats")]
        [SerializeField, Min(0)]
        [Tooltip("Max bullets per second this gun will fire")]
        protected float _fireRate = 1;
        
        [Header("References")]
        [SerializeField, HighlightIfNull]
        protected BulletBasic _bulletPrefab;
        
        [SerializeField, HighlightIfNull]
        protected Transform _gunTip;

        bool _firing;
        float _fireTimeStamp;

        void OnEnable() => SetFiringLoop();
        void OnDisable() => SetFiringLoop();

        /// <summary>
        /// Attempts to fire a bullet.
        /// </summary>
        /// <returns> The number of seconds before this gun can be fired again. </returns>
        public virtual float Fire()
        {
            // check cooldown
            float elapsedTime = Time.time - _fireTimeStamp; 
            float secondsPerBullet = 1 / _fireRate;
            if (elapsedTime <  secondsPerBullet) return secondsPerBullet - elapsedTime;
            
            // update cooldown
            _fireTimeStamp = Time.time;
            
            // fire bullet
            _bulletPrefab.Spawn(_gunTip.position, _gunTip.rotation);
            return secondsPerBullet;
        }

        Coroutine _firingLoop;
        void SetFiringLoop()
        {
            // stop loop
            if (!Firing)
            {
                if (_firingLoop != null)
                {
                    StopCoroutine(_firingLoop);
                    _firingLoop = null;
                }

                return;
            }
            
            // ignore, we are already looping
            if (_firingLoop != null) return;

            // start loop
            _firingLoop = StartCoroutine(Loop());
            
            IEnumerator Loop()
            {
                while (true)
                {
                    float cooldownAmount = Fire();
                    yield return new WaitForSeconds(cooldownAmount);
                }
            }
        }
    }
}
