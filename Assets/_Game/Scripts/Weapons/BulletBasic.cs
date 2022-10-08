using System.Collections;
using System.Collections.Generic;
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
        
        Rigidbody _rb;
        Vector3 initialPosition;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = true;
            
            initialPosition = transform.position;
        }

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

        protected virtual bool ShouldDespawn()
        {
            float distSq = (transform.position - initialPosition).sqrMagnitude;
            return distSq > _maxDistance * _maxDistance;
        }

        protected virtual void Despawn()
        {
            Destroy(gameObject);
        }
        
        protected virtual void Move()
        {
            Transform t = transform;
            Vector3 nPos = t.position + t.forward * (_speed * Time.deltaTime);
            
            _rb.MovePosition(nPos);
        }

        /// <summary>
        /// Creates a new instance of this bullet at <see cref="position"/> with <see cref="rotation"/>.
        /// </summary>
        /// <param name="position"> World-Space position </param>
        /// <param name="rotation"> World-Space rotation </param>
        /// <returns> Reference to this instance. </returns>
        public virtual BulletBasic Spawn(Vector3 position, Quaternion rotation)
        {
            return Instantiate(this, position, rotation);
        }
    }
}
