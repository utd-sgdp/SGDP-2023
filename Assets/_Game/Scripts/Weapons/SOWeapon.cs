using UnityEngine;

namespace Game.Weapons
{
    public abstract class SOWeapon : ScriptableObject
    {
        public string Name => _data.Name;
        public WeaponType WeaponType => _data.WeaponType;
        public float Damage => _data.Damage;
        public float Cooldown => _data.Cooldown;
        public FireMode FireMode => _data.FireMode;

        [SerializeField] protected WeaponData _data = new();

        public static SOWeapon Instantiate(WeaponData data)
        {
            return data.WeaponType switch
            {
                WeaponType.Melee => SOWeaponMelee.Instantiate(data),
                WeaponType.Ranged => SOWeaponRanged.Instantiate(data),
                _ => throw new System.ArgumentException($"Unknown weapon type: {data.WeaponType}", nameof(data)),
            };
        }

        protected static void LoadDefaultData(SOWeapon instance, WeaponData data)
        {
            instance.name = data.Name;
            instance._data.Name = data.Name;
            instance._data.WeaponType = data.WeaponType;
            instance._data.Damage = data.Damage;
            instance._data.Cooldown = data.Cooldown;
            instance._data.FireMode = data.FireMode;
        }
    }

    public enum WeaponType
    {
        Ranged = 0, Melee = 1,
    }

    [System.Serializable]
    public class WeaponData
    {
        // universal data
        public string Name;
        public WeaponType WeaponType;
        [Min(0)] public float Damage;
        [Min(0)] public float Cooldown;
        public FireMode FireMode;
        
        // ranged-specific data
        public Vector2 Spread;
        [Min(0)] public float Speed;
        public bool UseHitScan;
        public bool UseReload;
        [Min(0)] public int MagazineSize;
        public ReloadMode ReloadMode;
        [Min(0)] public int ReloadIncrement;
    }
}