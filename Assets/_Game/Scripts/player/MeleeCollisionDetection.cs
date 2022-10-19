using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player
{
    public class MeleeCollisionDetection : MonoBehaviour
    {
        public WeaponController wc;


        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Enemy" && wc.IsAttacking)
            {
                Debug.Log("hit");
            }
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
