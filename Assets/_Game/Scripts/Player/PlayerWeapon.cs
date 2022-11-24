using System;
using Game.Animation;
using Game.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerWeapon : MonoBehaviour, IWeaponProvider
    {
        #region Events
        public Action<ActionType> OnAction;
        public void Subscribe(Action<ActionType> callback) => OnAction += callback;
        public void Unsubscribe(Action<ActionType> callback) => OnAction -= callback;
        #endregion
        
        public WeaponBase Weapon => _weapon;
        [SerializeField]
        WeaponBase _weapon;

        PlayerInput _inputComponent;
        PlayerInput _input
        {
            get
            {
                if (_inputComponent == null)
                {
                    _inputComponent = GetComponent<PlayerInput>();
                }

                return _inputComponent;
            }
        }

        void OnEnable()
        {
            _input.actions["Fire"].performed += SetFiring;
            _weapon.OnAction += PropagateWeaponEvents;
        }

        void OnDisable()
        {
            _input.actions["Fire"].performed -= SetFiring;
            _weapon.OnAction -= PropagateWeaponEvents;
        }
        
        void PropagateWeaponEvents(ActionType type)
        {
            OnAction?.Invoke(type);
        }

        void SetFiring(InputAction.CallbackContext context)
        {
            float buttonValue = context.ReadValue<float>();
            bool buttonDown = buttonValue > 0.5f;

            if (buttonDown)
            {
                _weapon.AttemptAttack(true);
                return;
            }

            _weapon.Looping = false;
        }
    }
}