using System;
using System.Collections.Generic;
using System.IO;
using Game.Agent;
using UnityEditor.AssetImporters;

namespace GameEditor.Importers
{
    public static class EnemyImporter
    {
        public static void OnImportAsset(AssetImportContext ctx, string assetPath)
        {
            using StreamReader file = File.OpenText(assetPath);
            
            // skip headers at the top of the sheet
            file.ReadLine();
                 
            List<SOAgent> agents = new();
            
            // parse line-by-line
            string line = file.ReadLine();
            while (line != null)
            {
                string[] fields = line.Split("\t");
                     
                // ignore agents without names
                if (fields[0] == string.Empty)
                {
                    line = file.ReadLine();
                    continue;
                }
                     
                // parse line int weapon
                AgentData data = ParseLineForAgentData(fields);
                SOAgent agent = SOAgent.Instantiate(data);
                    
                // save weapon as sub-asset
                agents.Add(agent);
                ctx.AddObjectToAsset(data.Name, agent);
                         
                // increment to the next line
                line = file.ReadLine();
            }
                
            // organize final assets
            SOAgentPool pool = SOAgentPool.Instantiate(agents.ToArray());
            ctx.AddObjectToAsset("Agent Pool", pool);
                
            ctx.SetMainObject(pool);
        }
        
        static AgentData ParseLineForAgentData(in string[] fields)
        {
            AgentData data = new()
            {
                Name = Index(fields, 0),
                Class = Enum.Parse<AgentClass>(Index(fields, 1)),
            };
        
            data.MaxHealth._enabled = float.TryParse(Index(fields, 2), out data.MaxHealth.Value);
            data.MoveSpeed._enabled = float.TryParse(Index(fields, 3), out data.MoveSpeed.Value);
            data.TurnSpeed._enabled = float.TryParse(Index(fields, 4), out data.TurnSpeed.Value);
            data.Acceleration._enabled = float.TryParse(Index(fields, 5), out data.Acceleration.Value);
            data.AttackRange._enabled = float.TryParse(Index(fields, 6), out data.AttackRange.Value);
            data.SightRange._enabled = float.TryParse(Index(fields, 7), out data.SightRange.Value);
            // float.TryParse(Index(fields, 6), out data.Spread.y);
            // float.TryParse(Index(fields, 7), out data.Speed);
            // bool.TryParse(Index(fields, 8), out data.UseHitScan);
            // bool.TryParse(Index(fields, 9), out data.UseReload);
            // int.TryParse(Index(fields, 10), out data.MagazineSize);
            // Enum.TryParse(Index(fields, 11), out data.ReloadMode);
            // int.TryParse(Index(fields, 12), out data.ReloadIncrement);
            // if (!int.TryParse(Index(fields, 13), out data.PelletCount)) data.PelletCount = 1;
        
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