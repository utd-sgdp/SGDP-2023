using Game.Agent.Tree;
using PlasticPipe.PlasticProtocol.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Agent.Editor
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<NodeView> OnNodeSelected;
        public Tree.Node node;
        public Port input;
        public Port output;
        public NodeView(Tree.Node node) : base("Assets\\_Game\\Scripts\\Agent\\Editor\\NodeView.uxml")
        {
            //Store reference and title to be displayed
            this.node = node;
            this.title = node.name;
            this.viewDataKey = node.guid;

            //Set UI element's position to match the node's position
            style.left = node.editorPosition.x;
            style.top = node.editorPosition.y;

            //Create ports for edges to go to depending on type of node
            CreateInputPorts();
            CreateOutputPorts();
            SetupClasses();

            Label descriptionLabel = this.Q<Label>("description");
            descriptionLabel.bindingPath = "Description";
            descriptionLabel.Bind(new SerializedObject(node));
        }

        //Give each node class to change style of each type independently in editor
        private void SetupClasses()
        {
            if (node is Tree.ActionNode)
            {
                AddToClassList("action");
            }
            else if (node is Tree.CompositeNode)
            {
                AddToClassList("composite");
            }
            else if (node is Tree.DecoratorNode)
            {
                AddToClassList("decorator");
            }
            else if (node is Tree.RootNode)
            {
                AddToClassList("root");
            }
        }

        private void CreateInputPorts()
        {
            if (node is Tree.ActionNode)
            {
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is Tree.CompositeNode)
            {
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is Tree.DecoratorNode)
            {
                input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is Tree.RootNode)
            {
                
            }

            if (input != null)
            {
                input.portName = "";
                input.style.flexDirection = FlexDirection.Column; //Ports created during runtime so this modifies style during runtime
                inputContainer.Add(input);
            }
            
        }

        private void CreateOutputPorts()
        {
            if (node is Tree.ActionNode)
            {

            }
            else if (node is Tree.CompositeNode)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            else if (node is Tree.DecoratorNode)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else if (node is Tree.RootNode)
            {
                output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
            }

            if (output != null)
            {
                output.portName = "";
                output.style.flexDirection = FlexDirection.ColumnReverse; //Ports created during runtime so this modifies style during runtime
                outputContainer.Add(output);
            }
        }

        public override void OnSelected()
        {
            base.OnSelected();
            if (OnNodeSelected != null)
            {
                OnNodeSelected.Invoke(this);
            }
        }

        //Override method from GraphView
        public override void SetPosition(Rect newPos)
        {
            base.SetPosition(newPos);
            Undo.RecordObject(node, "Behaviour Tree (Set Position)");
            node.editorPosition.x = newPos.xMin;
            node.editorPosition.y = newPos.yMin;
            EditorUtility.SetDirty(node);
        }

        //Sort composite nodes to order children from position in editor left to right
        public void SortChildren()
        {
            CompositeNode composite = node as CompositeNode;
            if (composite)
            {
                composite.Children.Sort(SortByHorizontalPosition);
            }
        }

        private int SortByHorizontalPosition(Tree.Node left, Tree.Node right)
        {
            return left.editorPosition.x < right.editorPosition.y ? -1 : 1;
        }

        public void UpdateState()
        {
            RemoveFromClassList("running");
            RemoveFromClassList("success");
            RemoveFromClassList("failure");
            if (Application.isPlaying) {
                switch (node.CurrentState)
                {
                    case State.Running:
                        if (node.Started)
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
}
