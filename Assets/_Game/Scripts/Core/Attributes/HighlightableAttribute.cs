using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Attribute will color a field depending on a mode
    ///
    /// Colors: Multiple ways to set the color
    /// - Color Field is an Enum with only a few set colors
    /// - R G B is three floats that define a color by r, g, and b
    /// - System.Drawing.KnownColor is an enum with many color options
    /// Note: UnityEngine.Color does not work with Attributes
    ///
    /// HighlightMode: Defines the effect of the attribute
    /// - Back will highlight behind the text
    /// - Text will color the text directly
    /// 
    /// Credit: Brandon Coffey
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class HighlightableAttribute : PropertyAttribute
    {
        public readonly Color Color;
        public readonly HighlightMode Mode;

        protected HighlightableAttribute(ColorField color, HighlightMode mode = HighlightMode.Back)
        {
            Color = ConvertColor(color);
            Mode = mode;
        }

        protected HighlightableAttribute(float r, float g, float b, HighlightMode mode = HighlightMode.Back)
        {
            Color = new Color(r, g, b);
            Mode = mode;
        }

        protected HighlightableAttribute(int r, int g, int b, HighlightMode mode = HighlightMode.Back)
        {
            Color = new Color(r / 255f, g / 255f, b / 255f);
            Mode = mode;
        }

        protected HighlightableAttribute(System.Drawing.KnownColor color, HighlightMode mode = HighlightMode.Back)
        {
            var c = System.Drawing.Color.FromKnownColor(color);
            Color = new Color(c.R / 255f, c.B / 255f, c.G / 255f);
            Mode = mode;
        }

        public static Color ConvertColor(ColorField c)
        {
            return c switch
            {
                ColorField.Black => Color.black,
                ColorField.Blue => Color.blue,
                ColorField.Cyan => Color.cyan,
                ColorField.Gray => Color.gray,
                ColorField.Green => Color.green,
                ColorField.Grey => Color.grey,
                ColorField.Red => Color.red,
                ColorField.Magenta => Color.magenta,
                ColorField.White => Color.white,
                ColorField.Yellow => Color.yellow,
                _ => Color.white,
            };
        }
    }

    public enum ColorField
    {
        None,
        Black,
        Blue,
        Cyan,
        Gray,
        Green,
        Grey,
        Red,
        Magenta,
        White,
        Yellow, // More colors can be added below, make sure you add it to ConvertColor() as well
    }

    public enum HighlightMode
    {
        Back,
        Text
    }
}