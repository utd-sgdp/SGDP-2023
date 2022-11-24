#if UNITY_EDITOR
using Game;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Attributes
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return ShouldShow(property) ? EditorGUI.GetPropertyHeight(property, label, true) : 0;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!ShouldShow(property)) return;
            EditorGUI.PropertyField(position, property, label, true);
        }

        bool ShouldShow(SerializedProperty property)
        {
            if (attribute is not ShowIfAttribute attr) return true;

            var target = property.serializedObject.targetObject;
            return ShowIfEditorHelper.ShouldShow(target, attr.Targets);
        }
    }
}
#endif