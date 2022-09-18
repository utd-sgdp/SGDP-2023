// https://www.patrykgalach.com/2020/01/20/readonly-attribute-in-unity-editor/

using UnityEngine;
using UnityEditor;
using Game;

namespace GameEditor.Attributes
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Saving previous GUI enabled value
            var previousGUIState = GUI.enabled;

            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);

            // Setting old GUI enabled value
            GUI.enabled = previousGUIState;
        }
    }
}