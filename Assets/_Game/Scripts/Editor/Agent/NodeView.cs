using Game.Agent.Tree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameEditor.Agent
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<NodeView> OnNodeSelected;
        public Game.Agent.Tree.Node Node;
        public Port Input;
        public Port Output;
        
        public NodeView(Game.Agent.Tree.Node node) : base("Assets\\_Game\\Scripts\\Editor\\Agent\\NodeView.uxml")
        {
            // Store reference and title to be displayed
            Node = node;
            title = ObjectNames.NicifyVariableName(node.name);
            viewDataKey = node.guid;

            // Set UI element's position to match the node's position
            style.left = node.editorPosition.x;
            style.top = node.editorPosition.y;

            // Create ports for edges to go to depending on type of node
            CreateInputPorts();
            CreateOutputPorts();
            SetupClasses();

            Label descriptionLabel = this.Q<Label>("description");
            descriptionLabel.bindingPath = "Description";
            descriptionLabel.Bind(new SerializedObject(node));
        }

        // Give each node class to change style of each type independently in editor
        void SetupClasses()
        {
            switch (Node)
            {
                case ActionNode:
                    AddToClassList("action");
                    break;
                case CompositeNode:
                    AddToClassList("composite");
                    break;
                case DecoratorNode:
                    AddToClassList("decorator");
                    break;
                case RootNode:
                    AddToClassList("root");
                    break;
            }
        }

        void CreateInputPorts()
        {
            if (Node is ActionNode or CompositeNode or DecoratorNode)
            {
                Input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }

            // exit, this node has no input ports to configure
            if (Input == null) return;

            // Ports created during runtime so this modifies style during runtime
            Input.style.flexDirection = FlexDirection.Column;
            
            Input.portName = "";
            inputContainer.Add(Input);
        }

        void CreateOutputPorts()
        {
            if (Node is CompositeNode or DecoratorNode or RootNode)
            {
                Output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }

            // exit, this node has no output ports to configure
            if (Output == null) return;

            // Ports created during runtime so this modifies style during runtime
            Output.style.flexDirection = FlexDirection.ColumnReverse;
            
            Output.portName = "";
            outputContainer.Add(Output);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            OnNodeSelected?.Invoke(this);
        }

        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(Node, "Behaviour Tree (Set Position)");
            
            Node.editorPosition.x = newPos.xMin;
            Node.editorPosition.y = newPos.yMin;
            
            EditorUtility.SetDirty(Node);
        }

        /// <summary>
        /// Sorts composite nodes' children by horizontal position.
        /// </summary>
        public void SortChildren()
        {
            CompositeNode composite = Node as CompositeNode;
            if (!composite) return;
            
            composite.Children.Sort(SortByHorizontalPosition);
        }

        static int SortByHorizontalPosition(Game.Agent.Tree.Node left, Game.Agent.Tree.Node right)
        {
            return left.editorPosition.x < right.editorPosition.y ? -1 : 1;
        }

        public void UpdateState()
        {
            RemoveFromClassList("running");
            RemoveFromClassList("success");
            RemoveFromClassList("failure");

            // exit, the behaviour tree is not running.
            if (!Application.isPlaying)
            {
                return;
            }

            switch (Node.CurrentState)
            {
                case State.Running:
                    if (Node.Started)
                    {
                        AddToClassList("running");
                    }
                    break;
                case State.Success:
                    AddToClassList("success");
                    break;
                case State.Failure:
                    AddToClassList("failure");
                    break;
            }
        }
    }
}
