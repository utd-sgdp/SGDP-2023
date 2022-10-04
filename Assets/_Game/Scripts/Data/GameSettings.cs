using System.Collections;
using System.Collections.Generic;
using Game.Utility.Patterns;
using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "Game Settings", menuName = "Settings/Game Settings")]
    public class GameSettings : DynamicSingletonSO<GameSettings>
    {
        public AudioSettings Audio = new();
        public VideoSettings Video = new();

        public override string ToString()
        {
            return Audio + "\n" + Video + "\n";
        }

        // notes
        // Settings are auto read from user's computer or defaults
        // We need to perform saving manually with Save().
        //      This could be done on a button press and/or application quit.
        // Properties or setters should be used to modify Audio and Video to allow logic that applies those values
        // see Nick for more details
    }
}