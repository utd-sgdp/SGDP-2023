using System.Drawing;

namespace Game
{
    /// <summary>
    /// Attribute will color a field
    ///
    /// Usage:
    /// [SerializeField, Highlight(ColorField.Blue)] private GameObject _field;
    ///
    /// The field will be highlighted BLUE
    /// Custom colors or modes can be set, see HighlightableAttribute for info
    /// 
    /// Credit: Brandon Coffey
    /// </summary>
    public class HighlightAttribute : HighlightableAttribute
    {
        public HighlightAttribute() : base(ColorField.Green) {}
        public HighlightAttribute(ColorField color, HighlightMode mode = HighlightMode.Back) : base(color, mode) {}
        public HighlightAttribute(float r, float g, float b, HighlightMode mode = HighlightMode.Back) : base(r, g, b, mode) {}
        public HighlightAttribute(int r, int g, int b, HighlightMode mode = HighlightMode.Back) : base(r, g, b, mode) {}
        public HighlightAttribute(KnownColor color, HighlightMode mode = HighlightMode.Back) : base(color, mode) {}
    }
}