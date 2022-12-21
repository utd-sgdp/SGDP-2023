using UnityEngine;

namespace Game.Weapons
{
    public class SOWeaponPool : ScriptableObject
    {
        public SOWeapon[] Weapons => _weapons;
        [SerializeField] SOWeapon[] _weapons;

        public static SOWeaponPool Instantiate(SOWeapon[] weapons)
        {
            SOWeaponPool pool = CreateInstance<SOWeaponPool>();
            LoadData(pool, weapons);

            return pool;
        }

        static void LoadData(SOWeaponPool instance, SOWeapon[] weapons)
        {
            instance._weapons = weapons;
        }
    }
}