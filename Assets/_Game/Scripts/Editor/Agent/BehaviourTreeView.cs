using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System;
using System.Linq;
using Game.Agent.Tree;

namespace GameEditor.Agent
{
    public class BehaviourTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, UxmlTraits> { }

        BehaviourTree _tree;
        public Action<NodeView> OnNodeSelected;
        
        public BehaviourTreeView()
        {
            // Show grid background
            Insert(0, new GridBackground());

            // Add controls
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            // load stylesheet
            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_Game/Scripts/Editor/Agent/BehaviourTreeEditor.uss");
            styleSheets.Add(styleSheet);

            // enable undo/redo
            Undo.undoRedoPerformed += OnUndoRedo;
        }

        void OnUndoRedo()
        {
            PopulateView(_tree);
            AssetDatabase.SaveAssets();
        }

        NodeView FindNodeView(Game.Agent.Tree.Node node)
        {
            return GetNodeByGuid(node.guid) as NodeView;
        }

        internal void PopulateView(BehaviourTree tree)
        {
            _tree = tree;

            // Ignore events from graphView
            graphViewChanged -= OnGraphViewChanged;
            
            // Clear view of any previous tree
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            // create root node, if missing
            if (tree.RootNode == null)
            {
                tree.RootNode = tree.CreateNode(typeof(RootNode)) as RootNode;
                EditorUtility.SetDirty(tree);
                AssetDatabase.SaveAssets();
            }

            // render each node
            tree.nodes.ForEach(CreateNodeView);

            // render node edges
            foreach (var node in tree.nodes)
            {
                var children = node.GetChildren();
                foreach (var child in children)
                {
                    NodeView parentView = FindNodeView(node);
                    NodeView childView = FindNodeView(child);

                    Edge edge = parentView.Output.ConnectTo(childView.Input);
                    AddElement(edge);
                }
            }
        }

        GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            // delete nodes (and their children/edges)
            if (graphViewChange.elementsToRemove != null)
            {
                foreach (var elem in graphViewChange.elementsToRemove)
                {
                    if (elem is NodeView nodeView)
                    {
                        _tree.DeleteNode(nodeView.Node);
                    }

                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        NodeView parentView = edge.output.node as NodeView;
                        NodeView childView = edge.input.node as NodeView;
                        BehaviourTree.RemoveChild(parentView.Node, childView.Node);
                    }
                }
            }

            if (graphViewChange.edgesToCreate != null)
            {
                foreach (var edge in graphViewChange.edgesToCreate)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    BehaviourTree.AddChild(parentView.Node, childView.Node);
                    parentView.SortChildren();
                }
            }

            if (graphViewChange.movedElements != null)
            {
                foreach (var n in nodes)
                {
                    NodeView view = n as NodeView;
                    view.SortChildren();
                }
            }

            return graphViewChange;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            // base.BuildContextualMenu(evt);
            
            // show action nodes
            var types = TypeCache.GetTypesDerivedFrom<ActionNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", _ => CreateNode(type));
            }

            // show decorator nodes
            types = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", _ => CreateNode(type));
            }

            // show composite nodes
            types = TypeCache.GetTypesDerivedFrom<CompositeNode>();
            foreach (var type in types)
            {
                evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", _ => CreateNode(type));
            }
        }

        void CreateNode(Type type)
        {
            Game.Agent.Tree.Node node = _tree.CreateNode(type);
            CreateNodeView(node);
        }

        void CreateNodeView(Game.Agent.Tree.Node node)
        {
            // render node
            NodeView nodeView = new NodeView(node)
            {
                OnNodeSelected = OnNodeSelected,
            };
            AddElement(nodeView);
        }

        public void UpdateNodeStates()
        {
            foreach (var node in nodes)
            {
                NodeView view = node as NodeView;
                view.UpdateState();
            }
        }
    }
}
