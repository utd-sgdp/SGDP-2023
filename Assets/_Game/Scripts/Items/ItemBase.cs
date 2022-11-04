using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;
using UnityEngine.Events;

namespace Game.Items
{
    public abstract class ItemBase : MonoBehaviour
    {
        public LayerMask layer = ~0;

        public UnityEvent<TransformData> OnPickup;

        protected virtual void Pickup(PlayerStats player)
        {
            Destroy(gameObject, 0.01f);
        }

        void OnTriggerEnter(Collider other)
        {
            // if not on our target layer, ignore collision
            if (!layer.Includes(other.gameObject.layer)) return;

            var player = other.attachedRigidbody.GetComponent<PlayerStats>();
            if (!player) return;

            Pickup(player);
            OnPickup?.Invoke(transform);
        }
    }
}
