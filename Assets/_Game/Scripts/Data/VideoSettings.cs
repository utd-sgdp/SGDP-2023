using UnityEngine;

namespace Game.Data
{
    [System.Serializable]
    public class VideoSettings
    {
        [SerializeField] int Contrast;
        [SerializeField] int Brightness;
        [SerializeField] GraphicsQuality Quality = GraphicsQuality.Balanced;
        [SerializeField] bool VSync;

        public override string ToString()
        {
            return $"Video: [Contrast: {Contrast}, Brightness: {Brightness}, Quality: {Quality}, VSync: {VSync}]";
        }
    }

    public enum GraphicsQuality
    {
        Performant = 0, Balanced = 1, HighFidelity = 2
    }
}