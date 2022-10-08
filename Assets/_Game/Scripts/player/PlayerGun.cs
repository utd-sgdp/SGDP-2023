using System;
using Game.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerGun : MonoBehaviour
    {
        [SerializeField]
        GunBasic _gun;

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

            _gun.Firing = buttonDown;
        }
    }
}