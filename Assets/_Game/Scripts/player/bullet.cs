using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    
    public class bullet : MonoBehaviour
    {
        //bullet speed
        public float _speed;
        //how far the bullet can travel before being destroyed
        public float _maxDistance;
        private Vector3 initialPosition;
        // Start is called before the first frame update
        void Start()
        {
            initialPosition = transform.position;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //set velocity
            GetComponent<Rigidbody>().velocity = transform.forward * _speed;
            //check if the bullet is out of range
            if(Vector3.Distance(initialPosition, transform.position) > _maxDistance)
            {
                Destroy(gameObject);
            }
        }
    }
}
