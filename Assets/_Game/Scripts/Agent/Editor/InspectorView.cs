using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;
namespace Game.Agent.Editor
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        UnityEditor.Editor editor;

        public InspectorView()
        {

        }

        internal void UpdateSelection(NodeView nodeView)
        {
            //Clear inspector elements then display information about node
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);
            editor = UnityEditor.Editor.CreateEditor(nodeView.node);
            IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
            Add(container);
        }
    }
}
