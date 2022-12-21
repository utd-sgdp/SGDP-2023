using System;
using System.Collections.Generic;
using System.IO;
using Game.Weapons;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace GameEditor.Importers
{
    [ScriptedImporter(0, "tsv")]
    public class WeaponImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            // ignore tsv files that aren't weapons
            if (!HasBasePath(assetPath)) return;

            using (StreamReader file = File.OpenText(assetPath))
            {
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
        }
        
        const string BASE_PATH = "Assets/_Game/Design/Weapons";
        static bool HasBasePath(string assetPath) => assetPath.StartsWith(BASE_PATH);

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
