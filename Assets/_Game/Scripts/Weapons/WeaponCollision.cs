using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class WeaponCollision : MonoBehaviour
    {
        [Header("Debug")]
        [SerializeField, ReadOnly] WeaponBase _weapon;
        [SerializeField, ReadOnly] LayerMask _targetLayer = ~0;

        public void Configure(WeaponBase weapon, LayerMask targetLayers)
        {
            _weapon = weapon;
            _targetLayer = targetLayers;
        }

        public void Enable() => gameObject.SetActive(true);
        public void Disable() => gameObject.SetActive(false);
        
        void OnTriggerEnter(Collider other)
        {
            // ignore collision, other object is not on our target layer
            if (!_targetLayer.Includes(other.gameObject.layer)) return;
            
            Damageable target = other.gameObject.GetComponentInChildren<Damageable>();
            if (!target) return;
            
            // deal damage
            target.Hurt(_weapon.Damage * _weapon.Multiplier);
            print($"{_weapon.name} hit {other.gameObject.name} for {_weapon.Damage * _weapon.Multiplier} points.");
        }
    }
}
