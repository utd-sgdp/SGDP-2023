using UnityEngine;

namespace Game.Data
{
    [System.Serializable]
    public class AudioSettings
    {
        [SerializeField]              bool  Enabled = true;
        [SerializeField, Range(0, 1)] float Master = 1;
        [SerializeField, Range(0, 1)] float Music = 1;
        [SerializeField, Range(0, 1)] float Ambience = 1;
        [SerializeField, Range(0, 1)] float SFX = 1;
        [SerializeField, Range(0, 1)] float Gameplay = 1;
        [SerializeField, Range(0, 1)] float UI = 1;

        public override string ToString()
        {
            return $"Audio: [Enabled: {Enabled}, Master: {Master}, Music: {Music}, Ambience: {Ambience}, SFX: {SFX}, Gameplay: {Gameplay}, UI: {UI}]";
        }
    }
}