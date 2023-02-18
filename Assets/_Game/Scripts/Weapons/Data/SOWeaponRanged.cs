using UnityEngine;

namespace Game.Weapons
{
    public class SOWeaponRanged : SOWeapon
    {
        public Vector2 Spread => _data.Spread;
        public float Speed => _data.Speed;
        public bool UseHitScan => _data.UseHitScan;
        public bool UseReload => _data.UseReload;
        public int MagazineSize => _data.MagazineSize;
        public ReloadMode ReloadMode => _data.ReloadMode;
        public int ReloadIncrement => _data.ReloadIncrement;
        public int PelletCount => _data.PelletCount;
    }
}