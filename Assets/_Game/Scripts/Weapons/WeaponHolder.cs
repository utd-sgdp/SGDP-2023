using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class WeaponHolder : MonoBehaviour
    {
        public WeaponBase Value => _weapon;
        [SerializeField] WeaponBase _weapon;
    }
}
