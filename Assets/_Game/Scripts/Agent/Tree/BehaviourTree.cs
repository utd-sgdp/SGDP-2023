using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Agent.Tree
{
    [CreateAssetMenu]
    public class BehaviourTree : ScriptableObject
    {
        public Node RootNode;
        public State TreeState = State.Running;
        public Blackboard Blackboard;
        //Nodes in tree
        public List<Node> nodes = new List<Node>();

        public State Update()
        {
            if (RootNode.CurrentState is State.Running)
            {
                TreeState = RootNode.Update();
            }

            return TreeState;
        }

        /// <summary>
        /// Depth-first search of this <see cref="BehaviourTree"/>.
        /// </summary>
        /// <param name="node"> The node to traverse downwards from. </param>
        /// <param name="callback"> Callback performed at each node visited. </param>
        public static void Traverse(Node node, System.Action<Node> callback)
        {
            if (!node) return;
            
            callback?.Invoke(node);
            foreach (Node child in node.GetChildren())
            {
                Traverse(child, callback);
            }
        }

        /// <summary>
        /// Initializes <see cref="Blackboard"/> at/for runtime and binds it to all child nodes.
        /// </summary>
        public void Bind(GameObject gameObject, Transform transform)
        {
            Blackboard.RuntimeInitialize(gameObject, transform);
            
            Traverse(RootNode, node =>
            {
                node.Blackboard = Blackboard;
            });
        }

        public Node CreateNode(System.Type type)
        {
            Node node = ScriptableObject.CreateInstance(type) as Node;
            //Set name to appear in inspector
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();
            nodes.Add(node);

            //Makes ScriptableObject node subasset of BehaviourTree object
            AssetDatabase.AddObjectToAsset(node, this);
            AssetDatabase.SaveAssets();

            return node;
        }

        public void DeleteNode(Node node)
        {
            nodes.Remove(node);
            AssetDatabase.RemoveObjectFromAsset(node);
            AssetDatabase.SaveAssets();
        }

        //Methods to add and remove children to be called by OnGraphViewChanged when an edge is created between nodes
        public void AddChild(Node parent, Node child)
        {
            RootNode root = parent as RootNode;
            if (root)
            {
                root.Child = child;
            }

                DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                decorator.child = child;
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                composite.Children.Add(child);
            }
        }

        public void RemoveChild(Node parent, Node child)
        {
            RootNode root = parent as RootNode;
            if (root)
            {
                root.Child = null;
            }

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                decorator.child = null;
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                composite.Children.Remove(child);
            }
        }

        public List<Node> GetChildren(Node parent)
        {
            List<Node> children = new List<Node>();
            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator && decorator.child != null)
            {
                children.Add(decorator.child);
            }

            RootNode root = parent as RootNode;
            if (root && root.Child != null)
            {
                children.Add(root.Child);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                return composite.Children;
            }

            return children;
        }

        //Allow AIAgent to clone the tree for runtime usage to prevent multiple AIAgents using the same tree from conflicting.
        public BehaviourTree Clone()
        {
            BehaviourTree tree = Instantiate(this);
            tree.RootNode = tree.RootNode.Clone();
            return tree;
        }
    }
}
