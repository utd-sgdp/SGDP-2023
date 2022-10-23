using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using static UnityEditor.Experimental.GraphView.GraphView;
using System;
using System.Linq;

namespace Game.Agent.Editor
{
    public class BehaviourTreeView : GraphView
    {
        public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

        Tree.BehaviourTree tree;
        public Action<NodeView> OnNodeSelected;
        public BehaviourTreeView()
        {
            //Adds a grid to background
            Insert(0, new GridBackground());

            //Add Controls to editor window
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/_Game/Scripts/Agent/Editor/BehaviourTreeEditor.uss");
            styleSheets.Add(styleSheet);
        }

        NodeView FindNodeView(Tree.Node node)
        {
            return GetNodeByGuid(node.guid) as NodeView;
        }

        internal void PopulateView(Tree.BehaviourTree tree)
        {
            this.tree = tree;

            //Ignore events from graphView
            graphViewChanged -= OnGraphViewChanged;
            //Clear view of any previous tree
            DeleteElements(graphElements);
            graphViewChanged += OnGraphViewChanged;

            if (tree.RootNode == null)
            {
                tree.RootNode = tree.CreateNode(typeof(Tree.RootNode)) as Tree.RootNode;
                EditorUtility.SetDirty(tree);
                AssetDatabase.SaveAssets();
            }

            //Call CreateNodeView on each node of tree
            tree.nodes.ForEach(node => CreateNodeView(node));

            //Create edges between parents and children
            tree.nodes.ForEach(node => {
                var children = tree.GetChildren(node);
                children.ForEach(child => {
                    NodeView parentView = FindNodeView(node);
                    NodeView childView = FindNodeView(child);

                    Edge edge = parentView.output.ConnectTo(childView.input);
                    AddElement(edge);
                });
            });
        }

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            //Cast to NodeView then delete node
            if (graphViewChange.elementsToRemove != null)
            {
                graphViewChange.elementsToRemove.ForEach(elem =>
                {
                    NodeView nodeView = elem as NodeView;
                    if (nodeView != null)
                    {
                        tree.DeleteNode(nodeView.node);
                    }

                    Edge edge = elem as Edge;
                    if (edge != null)
                    {
                        NodeView parentView = edge.output.node as NodeView;
                        NodeView childView = edge.input.node as NodeView;
                        tree.RemoveChild(parentView.node, childView.node);
                    }
                });
            }

            if (graphViewChange.edgesToCreate != null)
            {
                graphViewChange.edgesToCreate.ForEach(edge => {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    tree.AddChild(parentView.node, childView.node);
                });
            }
            return graphViewChange;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            //base.BuildContextualMenu(evt);
            //Create dropdown menu with option for each type of node
            {
                var types = TypeCache.GetTypesDerivedFrom<Tree.ActionNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<Tree.DecoratorNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
                }
            }

            {
                var types = TypeCache.GetTypesDerivedFrom<Tree.CompositeNode>();
                foreach (var type in types)
                {
                    evt.menu.AppendAction($"[{type.BaseType.Name}] {type.Name}", (a) => CreateNode(type));
                }
            }
        }

        void CreateNode(System.Type type)
        {
            Tree.Node node = tree.CreateNode(type);
            CreateNodeView(node);
        }

        void CreateNodeView(Tree.Node node)
        {
            //Take in node and add it to the window
            NodeView nodeView = new NodeView(node);
            nodeView.OnNodeSelected =  OnNodeSelected;
            AddElement(nodeView);
        }
    }
}
