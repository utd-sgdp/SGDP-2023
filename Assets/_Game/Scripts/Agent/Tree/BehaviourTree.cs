using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

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

            Undo.RecordObject(this, "Behaviour Tree (CreateNode)");
            nodes.Add(node);

            //Makes ScriptableObject node subasset of BehaviourTree object
            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }
            Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");
            AssetDatabase.SaveAssets();

            return node;
        }

        public void DeleteNode(Node node)
        {
            Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");
            nodes.Remove(node);

            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
        }

        //Methods to add and remove children to be called by OnGraphViewChanged when an edge is created between nodes
        public void AddChild(Node parent, Node child)
        {
            RootNode root = parent as RootNode;
            if (root)
            {
                Undo.RecordObject(root, "Behaviour Tree (AddChild)");
                root.child = child;
                EditorUtility.SetDirty(root);
            }

                DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (AddChild)");
                decorator.child = child;
                EditorUtility.SetDirty(decorator);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
                composite.Children.Add(child);
                EditorUtility.SetDirty(composite);
            }
        }

        public void RemoveChild(Node parent, Node child)
        {
            RootNode root = parent as RootNode;
            if (root)
            {
                Undo.RecordObject(root, "Behaviour Tree (RemoveChild)");
                root.child = null;
                EditorUtility.SetDirty(root);
            }

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                Undo.RecordObject(decorator, "Behaviour Tree (RemoveChild)");
                decorator.child = null;
                EditorUtility.SetDirty(decorator);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                Undo.RecordObject(composite, "Behaviour Tree (RemoveChild)");
                composite.Children.Remove(child);
                EditorUtility.SetDirty(composite);
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
            if (root && root.child != null)
            {
                children.Add(root.child);
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                return composite.Children;
            }

            return children;
        }

        /**
        public void Traverse(Node node, System.Action<Node> visiter)
        {
            if (node)
            {
                visiter.Invoke(node);
                var children = GetChildren(node);
                children.ForEach((n) => Traverse(n, visiter));
            }
        }**/

        //Allow AIAgent to clone the tree for runtime usage to prevent multiple AIAgents using the same tree from conflicting.
        public BehaviourTree Clone()
        {
            BehaviourTree tree = Instantiate(this);
            tree.RootNode = tree.RootNode.Clone();
            tree.nodes = new List<Node>();
            Traverse(tree.RootNode, (n) =>
            {
                tree.nodes.Add(n);
            });
            return tree;
        }
    }
}
