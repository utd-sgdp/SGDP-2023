using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;
using Game.Agent.Tree;

namespace GameEditor.Agent
{
    public class BehaviourTreeEditor : EditorWindow
    {
        BehaviourTreeView treeView;
        InspectorView inspectorView;
        IMGUIContainer blackboardView;

        SerializedObject treeObject;
        SerializedProperty blackboardProperty;

        public static void OpenWindow()
        {
            BehaviourTreeEditor window = GetWindow<BehaviourTreeEditor>();
            window.titleContent = new GUIContent("BehaviourTreeEditor");
        }
        
        /// <summary>
        /// Opens editor window, if a BehaviourTree has been double clicked
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is not BehaviourTree) return false;

            OpenWindow();
            return true;
        }

        public void CreateGUI()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // Import UXML
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Game/Scripts/Editor/Agent/BehaviourTreeEditor.uxml");
            visualTree.CloneTree(root);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_Game/Scripts/Editor/Agent/BehaviourTreeEditor.uss");
            root.styleSheets.Add(styleSheet);

            treeView = root.Q<BehaviourTreeView>();
            inspectorView = root.Q<InspectorView>();
            blackboardView = root.Q<IMGUIContainer>();
            blackboardView.onGUIHandler = () =>
            {
                if (treeObject == null) return;
                treeObject.Update();
                EditorGUILayout.PropertyField(blackboardProperty);
                treeObject.ApplyModifiedProperties();
            };

            treeView.OnNodeSelected = OnNodeSelectionChanged;
            OnSelectionChange();
        }

        void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            switch (obj)
            {
                case PlayModeStateChange.EnteredEditMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingEditMode:
                    break;
                case PlayModeStateChange.EnteredPlayMode:
                    OnSelectionChange();
                    break;
                case PlayModeStateChange.ExitingPlayMode:
                    break;
            }
        }

        void OnSelectionChange()
        {
            BehaviourTree tree = Selection.activeObject as BehaviourTree;

            if (!tree)
            {
                // TODO: ???????
                if (Selection.activeGameObject)
                {
                    AIAgent runner = Selection.activeGameObject.GetComponent<AIAgent>();
                    if (runner)
                    {
                        tree = runner.Tree;
                    }
                }
            }

            if (Application.isPlaying)
            {
                if (!tree || treeView == null) return;
                treeView.PopulateView(tree);
            }
            else
            {
                if (tree && treeView != null && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
                {
                    treeView.PopulateView(tree);
                }
            }

            if (tree == null) return;

            treeObject = new SerializedObject(tree);
            blackboardProperty = treeObject.FindProperty("Blackboard");
        }

        void OnNodeSelectionChanged(NodeView node)
        {
            inspectorView.UpdateSelection(node);
        }

        public void OnInspectorUpdate()
        {
            treeView?.UpdateNodeStates();
        }
    }
}