#if UNITY_EDITOR
using UnityEditor;

namespace GameEditor.Attributes.Buttons
{
    using Object = UnityEngine.Object;

    [CustomEditor(typeof(Object), true)]
    [CanEditMultipleObjects]
    internal class ButtonObjectEditor : UnityEditor.Editor
    {
        private ButtonsDrawer _buttonsDrawer;

        private void OnEnable() {
            _buttonsDrawer = new ButtonsDrawer(target);
        }

        public override void OnInspectorGUI() {
            DrawDefaultInspector();
            _buttonsDrawer.DrawButtons(targets);
        }
    }
}
#endif