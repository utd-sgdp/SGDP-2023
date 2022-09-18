using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Attribute disables the ability to edit properties and fields editor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class ReadOnlyAttribute : PropertyAttribute { }
}