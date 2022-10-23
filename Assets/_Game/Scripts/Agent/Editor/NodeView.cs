using PlasticPipe.PlasticProtocol.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Game.Agent.Editor
{
    public class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public Action<NodeView> OnNodeSelected;
        public Tree.Node node;
        public Port input;
        public Port output;
        public NodeView(Tree.Node node)
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
        }

        private void CreateInputPorts()
        {
            if (node is Tree.ActionNode)
            {
                input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is Tree.CompositeNode)
            {
                input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is Tree.DecoratorNode)
            {
                input = InstantiatePort(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(bool));
            }
            else if (node is Tree.RootNode)
            {
                
            }

            if (input != null)
            {
                input.portName = "";
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
                output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Multi, typeof(bool));
            }
            else if (node is Tree.DecoratorNode)
            {
                output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            }
            else if (node is Tree.RootNode)
            {
                output = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
            }

            if (output != null)
            {
                output.portName = "";
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
            node.editorPosition.x = newPos.xMin;
            node.editorPosition.y = newPos.yMin;
        }
    }
}
