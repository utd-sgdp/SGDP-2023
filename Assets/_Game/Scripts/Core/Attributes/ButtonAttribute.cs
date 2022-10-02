using System;

namespace Game
{
    /// <summary>
    /// Attribute expose a function to the Inspector as a clickable button
    ///
    /// Usage:
    /// [Button]
    /// private void TestFunction() {
    ///     Debug.Log("Test");
    /// }
    /// When the button is pressed in the inspector, the code will run and 'Test' will be printed
    ///
    /// [Button(Mode = ButtonMode.InPlayMode]
    ///     - The button is only clickable when the game is playing
    /// [Button(Mode = ButtonMode.NotInPlayMode]
    ///     - The button is only clickable when the game is not playing
    /// 
    /// If a custom editor is in use for the script, this attribute will not work
    /// 
    /// Credit: Brandon Coffey
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ButtonAttribute : Attribute
    {
        public string Label { get; set; } = "";
        public ButtonMode Mode { get; set; } = ButtonMode.Always;
        public int Spacing { get; set; } = 0;
        public ColorField Color { get; set; } = ColorField.None;
    }

    public enum ButtonMode
    {
        Always,
        InPlayMode,
        NotInPlayMode
    }
}