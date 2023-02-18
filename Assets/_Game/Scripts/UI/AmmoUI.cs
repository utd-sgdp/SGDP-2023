using System;
using Game.Player;
using Game.Weapons;
using TMPro;
using UnityEngine;

namespace Game
{
    public class AmmoUI : MonoBehaviour
    {
        // [SerializeField] PlayerWeapon _player;

        [Header("References")]
        [SerializeField] TMP_Text _txt_current;
        [SerializeField] TMP_Text _txt_max;

        // void OnEnable()
        // {
        //     // exit, there is no gun to 
        //     if (!_player || !_player.Weapon) return;
        //
        //     GunBasic gun = _player.Weapon as GunBasic;
        //     if (!gun) return;
        // }

        [Button(Mode = ButtonMode.InPlayMode)]
        void UpdateUI(int current, int max)
        {
            _txt_current.text = current.ToString();
            _txt_max.text = max.ToString();
        }
    }
}
