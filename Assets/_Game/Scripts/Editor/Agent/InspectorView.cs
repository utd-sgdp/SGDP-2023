using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;
namespace GameEditor.Agent
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

        UnityEditor.Editor editor;

        public InspectorView() { }

        internal void UpdateSelection(NodeView nodeView)
        {
            //Clear inspector elements then display information about node
            Clear();

            UnityEngine.Object.DestroyImmediate(editor);
            editor = UnityEditor.Editor.CreateEditor(nodeView.node);
            //Renders object if it still exists
            IMGUIContainer container = new IMGUIContainer(() => {
                if (editor.target)
                {
                    editor.OnInspectorGUI();
                }
            });
            Add(container);
        }
    }
}
