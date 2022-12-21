using UnityEngine;

namespace Game.Weapons
{
    public class SOWeaponMelee : SOWeapon
    {
        public new static SOWeaponMelee Instantiate(WeaponData data)
        {
            SOWeaponMelee instance = CreateInstance<SOWeaponMelee>();
            LoadDefaultData(instance, data);

            return instance;
        }
    }
}