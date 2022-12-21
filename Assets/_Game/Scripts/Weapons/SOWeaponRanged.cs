using NUnit.Framework;

namespace Game.Weapons
{
    public class SOWeaponRanged : SOWeapon
    {
        public new static SOWeaponRanged Instantiate(WeaponData data)
        {
            SOWeaponRanged instance = CreateInstance<SOWeaponRanged>();
            LoadDefaultData(instance, data);
            LoadSpecificData(instance, data);

            return instance;
        }

        static void LoadSpecificData(SOWeaponRanged instance, WeaponData data)
        {
            instance._data.Spread = data.Spread;
            instance._data.Speed = data.Speed;
            instance._data.UseHitScan = data.UseHitScan;
            instance._data.UseReload = data.UseReload;
            instance._data.MagazineSize = data.MagazineSize;
            instance._data.ReloadMode = data.ReloadMode;
            instance._data.ReloadIncrement = data.ReloadIncrement;
        }
    }
}