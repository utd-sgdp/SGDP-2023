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
            //if the player was attacking and the weapon hit an enemy
            if(other.tag == "Enemy" && wc.IsAttacking)
            {
                Debug.Log("hit");
            }
        }
    }
}
