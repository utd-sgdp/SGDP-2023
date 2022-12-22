using GameEditor.Importers;
using UnityEditor.AssetImporters;

namespace GameEditor
{
    [ScriptedImporter(0, "tsv")]
    public class TSVImporters : ScriptedImporter
    {
        const string PATH_ENEMIES = "Assets/_Game/Design/Enemies";
        const string PATH_WEAPONS = "Assets/_Game/Design/Weapons";
        
        public override void OnImportAsset(AssetImportContext ctx)
        {
            if (ctx.assetPath.StartsWith(PATH_WEAPONS))
            {
                WeaponImporter.OnImportAsset(ctx, assetPath);
            }
            else if (ctx.assetPath.StartsWith(PATH_ENEMIES))
            {
                EnemyImporter.OnImportAsset(ctx, assetPath);
            }
        }
    }
}
