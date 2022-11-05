using System;
using Game.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerWeapon : MonoBehaviour
    {
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
        }

        void OnDisable()
        {
            _input.actions["Fire"].performed -= SetFiring;
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