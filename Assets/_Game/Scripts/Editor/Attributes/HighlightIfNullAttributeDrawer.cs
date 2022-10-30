#if UNITY_EDITOR
using Game;
using UnityEditor;

namespace GameEditor.Attributes
{
    [CustomPropertyDrawer(typeof(HighlightIfNullAttribute))]
    public class HighlightIfNullAttributeDrawer : HighlightableAttributeDrawer
    {
        protected override bool ShouldHighlight(SerializedProperty property)
        {
            return property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue == null;
        }
    }
}
#endif