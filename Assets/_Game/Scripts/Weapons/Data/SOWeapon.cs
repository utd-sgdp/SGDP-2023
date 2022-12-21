using UnityEngine;

namespace Game.Weapons
{
    public enum WeaponType
    {
        Ranged = 0, Melee = 1,
    }
    
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
            SOWeapon instance = data.WeaponType switch
            {
                WeaponType.Melee => CreateInstance<SOWeaponMelee>(),
                WeaponType.Ranged => CreateInstance<SOWeaponRanged>(),
                _ => throw new System.ArgumentException($"Unknown weapon type: {data.WeaponType}", nameof(data)),
            };

            instance._data = data;
            instance.name = data.Name;
            
            return instance;
        }
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
        [Min(1)] public int ReloadIncrement;
        [Min(1)] public int PelletCount;
    }
}