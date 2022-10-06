using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    
    public class BulletBasic : MonoBehaviour
    {
        //bullet speed
        [SerializeField]
        float _speed;
        //how far the bullet can travel before being destroyed
        [SerializeField]
        float _maxDistance;
        private Vector3 initialPosition;
        Rigidbody _bulletRigidbody;

        void Awake()
        {
            initialPosition = transform.position;
            _bulletRigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //set velocity
            _bulletRigidbody.velocity = transform.forward * _speed;
            //check if the bullet is out of range
            if(Vector3.Distance(initialPosition, transform.position) > _maxDistance)
            {
                Destroy(gameObject);
            }
        }

        public virtual BulletBasic Spawn(Vector3 position, Quaternion rotation)
        {
            return Instantiate(this, position, rotation);
        }
    }
}
