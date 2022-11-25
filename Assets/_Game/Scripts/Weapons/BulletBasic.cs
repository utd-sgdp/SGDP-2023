using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Play;
using Game.Utility;
using UnityEngine;

namespace Game.Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletBasic : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Meters per second the bullet travels.")]
        float _speed;
        
        [SerializeField]
        [Tooltip("Distance from initial position this bullet may move, before being destroyed.")]
        float _maxDistance;

        [SerializeField]
        bool _hurtOnCollision = true;

        [SerializeField, HideInInspector] Rigidbody _rb;
        [SerializeField, HideInInspector] Collider _collider;

        Collider[] _sourceColliders = Array.Empty<Collider>();
        float _damage;
        Pool _pool;
        Vector3 _initialPosition;

        #region MonoBehaviour
        #if UNITY_EDITOR
        void Awake()
        {
            _collider = GetComponentInChildren<Collider>();
            _rb = GetComponent<Rigidbody>();
        }
        #endif

        void FixedUpdate()
        {
            if (ShouldDespawn())
            {
                Despawn();
                return;
            }

            // move bullet
            Move();
        }

        void OnCollisionEnter(Collision collision)
        {
            if (_sourceColliders.Contains(collision.collider)) return;
            Hit(collision.collider);
        }
        #endregion
        
        public void Configure(TransformData origin, Collider[] sourceColliders, float damage, float spread = 0, Pool pool = null)
        {
            _sourceColliders = sourceColliders;
            _pool = pool;
            _damage = damage;
            
            Vector3 position = origin.Position;
            Quaternion rotation = origin.Rotation;

            rotation = ApplySpread(rotation, spread);
            
            _initialPosition = position;
            transform.SetPositionAndRotation(position, rotation);
            
            // prevent artifacts in the trail renderer caused by object pooling
            foreach (var trail in GetComponentsInChildren<TrailRenderer>())
            {
                trail.Clear();
            }
        }

        protected virtual bool ShouldDespawn()
        {
            float distSq = (transform.position - _initialPosition).sqrMagnitude;
            return distSq > _maxDistance * _maxDistance;
        }

        protected virtual void Hit(Collider other)
        {
            // TODO: play vfx/sfx

            if (_hurtOnCollision)
            {
                Damageable target = Damageable.Find(other);
                if (target)
                {
                    target.Hurt(_damage);
                }
            }
            
            Despawn();
        }
        
        protected virtual void Despawn()
        {
            if (_pool)
            {
                _pool.CheckIn(gameObject);
                return;
            }
            
            Destroy(gameObject);
        }
        
        protected virtual void Move()
        {
            Transform t = transform;
            Vector3 nPos = t.position + t.forward * (_speed * Time.deltaTime);
            
            _rb.MovePosition(nPos);
        }

        public static bool HitScan(TransformData origin, out RaycastHit hitinfo, float spread=0)
        {
            Vector3 position = origin.Position;
            Quaternion rotation = origin.Rotation;
            
            Vector3 direction = ApplySpread(rotation, spread) * Vector3.forward;
            
            return Physics.Raycast(position, direction, out hitinfo, HIT_SCAN_MAX_DIST);
        }

        static Quaternion ApplySpread(Quaternion direction, float spread)
        {
            if (spread == 0) return direction;
            return direction.Randomize(new Vector3(spread, spread, 0));
        }

        const float HIT_SCAN_MAX_DIST = 100f;
    }
}
