using System;
using System.Collections.Generic;
using System.IO;
using Game.Weapons;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace GameEditor.Importers
{
    public static class WeaponImporter
    {
        public static void OnImportAsset(AssetImportContext ctx, string assetPath)
        {
            using StreamReader file = File.OpenText(assetPath);
            
            // skip headers at the top of the sheet
            file.ReadLine();
                
            List<SOWeapon> weapons = new();

            // parse line-by-line
            string line = file.ReadLine();
            while (line != null)
            {
                string[] fields = line.Split("\t");
                    
                // ignore weapons without names
                if (fields[0] == string.Empty)
                {
                    line = file.ReadLine();
                    continue;
                }
                    
                // parse line int weapon
                WeaponData data = ParseLineForWeaponData(fields);
                SOWeapon weapon = SOWeapon.Instantiate(data);
                    
                // save weapon as sub-asset
                weapons.Add(weapon);
                ctx.AddObjectToAsset(data.Name, weapon);
                    
                // increment to the next line
                line = file.ReadLine();
            }
                
            // organize final assets
            SOWeaponPool pool = SOWeaponPool.Instantiate(weapons.ToArray());
            ctx.AddObjectToAsset("Weapon Pool", pool);
                
            ctx.SetMainObject(pool);
        }
        
        static WeaponData ParseLineForWeaponData(in string[] fields)
        {
            WeaponData data = new()
            {
                Name = Index(fields, 0),
                WeaponType = Enum.Parse<WeaponType>(Index(fields, 1)),
                Damage = float.Parse(Index(fields, 2)),
                Cooldown = float.Parse(Index(fields, 3)),
                FireMode = Enum.Parse<FireMode>(Index(fields, 4)),
                Spread = new Vector2(0, 0),
            };

            float.TryParse(Index(fields, 5), out data.Spread.x);
            float.TryParse(Index(fields, 6), out data.Spread.y);
            float.TryParse(Index(fields, 7), out data.Speed);
            bool.TryParse(Index(fields, 8), out data.UseHitScan);
            bool.TryParse(Index(fields, 9), out data.UseReload);
            int.TryParse(Index(fields, 10), out data.MagazineSize);
            Enum.TryParse(Index(fields, 11), out data.ReloadMode);
            int.TryParse(Index(fields, 12), out data.ReloadIncrement);
            if (!int.TryParse(Index(fields, 13), out data.PelletCount)) data.PelletCount = 1;

            return data;
        }

        static string Index(in string[] fields, int index)
        {
            return index < fields.Length
                ? fields[index]
                : null;
        }
    }
}
