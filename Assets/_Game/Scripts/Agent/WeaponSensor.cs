using System;
using System.Collections;
using System.Collections.Generic;
using Game.Animation;
using Game.Weapons;
using UnityEngine;

namespace Game.Agent
{
    public class WeaponSensor : MonoBehaviour, IWeaponProvider
    {
        #region Events
        public Action<ActionType> OnAction;
        public void Subscribe(Action<ActionType> callback) => OnAction += callback;
        public void Unsubscribe(Action<ActionType> callback) => OnAction -= callback;
        #endregion
        
        public WeaponBase Value => _weapon;
        [SerializeField] WeaponBase _weapon;

        void OnEnable()
        {
            _weapon.OnAction += PropagateWeaponEvents;
        }

        void OnDisable()
        {
            _weapon.OnAction -= PropagateWeaponEvents;
        }

        void PropagateWeaponEvents(ActionType type)
        {
            OnAction?.Invoke(type);
        }
    }
}
