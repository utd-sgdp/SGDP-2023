using System.Drawing;

namespace Game
{
    /// <summary>
    /// Attribute will color a field if the field contains a Null value
    ///
    /// Usage:
    /// [SerializeField, HighlightIfNull] private GameObject _obj;
    ///
    /// The field will be highlighted RED if '_obj' is null.
    /// Custom colors or modes can be set, see HighlightableAttribute for info
    /// 
    /// Credit: Brandon Coffey
    /// </summary>
    public class HighlightIfNullAttribute : HighlightableAttribute
    {
        public HighlightIfNullAttribute() : base(ColorField.Red) { }
        public HighlightIfNullAttribute(ColorField color, HighlightMode mode = HighlightMode.Back) : base(color, mode) {}
        public HighlightIfNullAttribute(float r, float g, float b, HighlightMode mode = HighlightMode.Back) : base(r, g, b, mode) {}
        public HighlightIfNullAttribute(int r, int g, int b, HighlightMode mode = HighlightMode.Back) : base(r, g, b, mode) {}
        public HighlightIfNullAttribute(KnownColor color, HighlightMode mode = HighlightMode.Back) : base(color, mode) {}
    }
}