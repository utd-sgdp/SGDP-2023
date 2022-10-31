using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    public abstract class ItemBase : MonoBehaviour
    {
        public LayerMask layer = ~0;

        public abstract void pickup(GameObject player);

        private void OnTriggerEnter(Collider other)
        {
            // if not on our target layer, ignore collision
            if (!layer.Includes(other.gameObject.layer)) return;

            pickup(other.gameObject);
            Destroy(gameObject);
        }
    }
}
