using System.Drawing;

namespace Game
{
    /// <summary>
    /// Attribute will color a field if another field is set to true
    ///
    /// Usage:
    /// [SerializeField] private bool _useObj;
    /// [SerializeField, HighlightIfNull("_useObj")] private GameObject _obj;
    ///
    /// The field, '_obj', will be highlighted GREEN if '_useObj' is true.
    /// For info on how the IF part works, see ShowIfAttribute for info
    /// Custom colors or modes can be set, see HighlightableAttribute for info
    /// 
    /// Credit: Brandon Coffey
    /// </summary>
    public class HighlightIfAttribute : HighlightableAttribute
    {
        public readonly string[] Targets;

        public HighlightIfAttribute(params string[] targets) : base(ColorField.Green) { Targets = targets; }
        public HighlightIfAttribute(ColorField color, params string[] targets) : base(color) { Targets = targets; }
        public HighlightIfAttribute(ColorField color, HighlightMode mode = HighlightMode.Back, params string[] targets) : base(color, mode) { Targets = targets; }
        public HighlightIfAttribute(float r, float g, float b, params string[] targets) : base(r, g, b) { Targets = targets; }
        public HighlightIfAttribute(float r, float g, float b, HighlightMode mode = HighlightMode.Back, params string[] targets) : base(r, g, b, mode) { Targets = targets; }
        public HighlightIfAttribute(int r, int g, int b, params string[] targets) : base(r, g, b) { Targets = targets; }
        public HighlightIfAttribute(int r, int g, int b, HighlightMode mode = HighlightMode.Back, params string[] targets) : base(r, g, b, mode) { Targets = targets; }
        public HighlightIfAttribute(KnownColor color, params string[] targets) : base(color) { Targets = targets; }
        public HighlightIfAttribute(KnownColor color, HighlightMode mode = HighlightMode.Back, params string[] targets) : base(color, mode) { Targets = targets; }
    }
}