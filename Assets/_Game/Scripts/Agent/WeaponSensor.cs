using System.Collections;
using System.Collections.Generic;
using Game.Weapons;
using UnityEngine;

namespace Game.Agent
{
    public class WeaponSensor : MonoBehaviour
    {
        public WeaponBase Value => _weapon;
        [SerializeField] WeaponBase _weapon;
    }
}
