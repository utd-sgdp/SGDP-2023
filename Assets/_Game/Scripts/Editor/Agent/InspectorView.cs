using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;
namespace GameEditor.Agent
{
    public class InspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<InspectorView, UxmlTraits> { }

        Editor _editor;

        internal void UpdateSelection(NodeView nodeView)
        {
            // remove previous selection's info
            Clear();

            // update editor reference
            UnityEngine.Object.DestroyImmediate(_editor);
            _editor = Editor.CreateEditor(nodeView.Node);
            
            // render object
            IMGUIContainer container = new IMGUIContainer(() => {
                if (!_editor.target) return;
                _editor.OnInspectorGUI();
            });
            
            Add(container);
        }
    }
}
