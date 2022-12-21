using System;
using System.Collections;
using System.Linq;
using Game.Animation;
using Game.Play;
using Game.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Weapons
{
    public enum ReloadMode 
    {
        Magazine,
        Increment,
    }
    
    public class GunBasic : WeaponBase
    {
        public int MagazineSize => _magazineSize;
        public int BulletsLeft => _bulletsLeft;

        [Header("Stats")]
        [SerializeField]
        protected Optional<float> _spread = new();

        [SerializeField]
        protected bool _hitScan;
        
        [SerializeField]
        bool _reloadEnabled = true;
        
        [SerializeField, ShowIf(nameof(_reloadEnabled))]
        protected int _magazineSize;
        
        [SerializeField, ShowIf(nameof(_reloadEnabled))]
        protected float _reloadTime;
        

        [SerializeField, ShowIf(nameof(_reloadEnabled))]
        protected ReloadMode _reloadMode;
        
        [SerializeField, ReadOnly]
        protected int _bulletsLeft;

        [Header("References")]
        [SerializeField, HighlightIfNull]
        protected Pool _bulletPool;
        
        [SerializeField, HighlightIfNull]
        protected Transform _gunTip;

        public UnityEvent<TransformData> OnFire;

        bool _reloading;
        [SerializeField, HideInInspector] Collider[] _colliders = Array.Empty<Collider>();

        #region MonoBehaviour
        protected override void Awake()
        {
            base.Awake();
            
            _bulletsLeft = _reloadEnabled ? _magazineSize : int.MaxValue;
        }
        
        #if UNITY_EDITOR
        void OnValidate()
        {
            _colliders = _colliders.Where(col => col).ToArray();
            
            Rigidbody rb = GetComponentInParent<Rigidbody>();
            if (!rb) return;
            
            _colliders = rb.GetComponentsInChildren<Collider>();
        }
        #endif
        #endregion

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
                if (_reloadMode == ReloadMode.Increment)
                {
                    _reloading = false;
                    return true;
                }
                
                return false;
            }

            // start reloading
            if (_reloadEnabled && _bulletsLeft <= 0)
            {
                Reload();
                return false;
            }

            return true;
        }

        protected void Fire()
        {
            float spread = _spread.Enabled ? _spread.Value : 0;
            
            if (_hitScan) HitScan(spread);

            GameObject go = _bulletPool.CheckOut();
            BulletBasic bullet = go.GetComponent<BulletBasic>();
            bullet.Configure(_gunTip, _colliders, Damage, spread, _bulletPool);
            
            OnFire?.Invoke(_gunTip);
        }

        protected void HitScan(float spread)
        {
            // perform ray cast
            bool hit = BulletBasic.HitScan(_gunTip, out RaycastHit hitinfo, spread);
            if (!hit) return;
            
            // exit, target cannot be damaged
            Damageable target = Damageable.Find(hitinfo.collider);
            if (!target) return;
         
            target.Hurt(Damage);
        }

        /// <summary>
        /// Reload method encapsulating the separate implementations of the reload functions
        /// </summary>
        void Reload()
        {
            switch (_reloadMode)
            {
                default:
                case ReloadMode.Magazine:
                    StartCoroutine(magazineReload());
                    break;
                
                case ReloadMode.Increment:
                    StartCoroutine(incrementReload());
                    break;
            }
            
            OnAction?.Invoke(ActionType.Reload);
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