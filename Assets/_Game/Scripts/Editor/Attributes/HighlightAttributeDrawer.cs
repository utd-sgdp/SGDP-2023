#if UNITY_EDITOR
using Game;
using UnityEditor;

namespace GameEditor.Attributes
{
    [CustomPropertyDrawer(typeof(HighlightAttribute))]
    public class HighlightAttributeDrawer : HighlightableAttributeDrawer
    {
        protected override bool ShouldHighlight(SerializedProperty property) => true;
    }
}
#endif