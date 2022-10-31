using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Agent.Tree
{
    [CreateAssetMenu(menuName = "Agent/Tree")]
    public class BehaviourTree : ScriptableObject
    {
        [HideInInspector] public Node RootNode;
        [HideInInspector] public State TreeState = State.Running;
        [HideInInspector] public Blackboard Blackboard;
        [HideInInspector] public List<Node> nodes = new();

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
        public static void Traverse(Node node, Action<Node> callback)
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

        public Node CreateNode(Type type)
        {
            Node node = CreateInstance(type) as Node;
            nodes.Add(node);
            
            #if UNITY_EDITOR
            // Set name to appear in inspector
            // ReSharper disable once PossibleNullReferenceException
            node.name = type.Name;
            node.guid = GUID.Generate().ToString();

            Undo.RecordObject(this, "Behaviour Tree (CreateNode)");

            // Makes node sub-asset of BehaviourTree
            if (!Application.isPlaying)
            {
                AssetDatabase.AddObjectToAsset(node, this);
            }
            Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");
            AssetDatabase.SaveAssets();
            #endif

            return node;
        }

        public void DeleteNode(Node node)
        {
            nodes.Remove(node);
            
            #if UNITY_EDITOR
            Undo.RecordObject(this, "Behaviour Tree (DeleteNode)");

            //AssetDatabase.RemoveObjectFromAsset(node);
            Undo.DestroyObjectImmediate(node);
            AssetDatabase.SaveAssets();
            #endif
        }

        /// <summary>
        /// Adds node to tree. Can be called by OnGraphViewChanged to connect nodes.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public static void AddChild(Node parent, Node child)
        {
            bool isValid = false;
            RootNode root = parent as RootNode;
            if (root)
            {
                root.child = child;
                isValid = true;
            }

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                decorator.child = child;
                isValid = true;
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                composite.Children.Add(child);
                isValid = true;
            }

            #if UNITY_EDITOR
            if (isValid)
            {
                Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
                EditorUtility.SetDirty(composite);
            }
            #endif
        }

        /// <summary>
        /// Removes node from tree. Can be called by OnGraphViewChanged to disconnect or remove nodes.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        public static void RemoveChild(Node parent, Node child)
        {
            bool isValid = false;
            RootNode root = parent as RootNode;
            if (root)
            {
                root.child = null;
                isValid = true;
            }

            DecoratorNode decorator = parent as DecoratorNode;
            if (decorator)
            {
                decorator.child = null;
                isValid = true;
            }

            CompositeNode composite = parent as CompositeNode;
            if (composite)
            {
                composite.Children.Remove(child);
                isValid = true;
            }
            
            #if UNITY_EDITOR
            if (isValid)
            {
                Undo.RecordObject(composite, "Behaviour Tree (RemoveChild)");
                EditorUtility.SetDirty(composite);
            }
            #endif
        }

        /// <summary>
        /// Creates deep copy of this <see cref="BehaviourTree"/>. Useful to clone at runtime to prevent errors caused by
        /// multiple agents using the same tree.
        /// </summary>
        /// <returns></returns>
        public BehaviourTree Clone()
        {
            BehaviourTree tree = Instantiate(this);
            tree.RootNode = tree.RootNode.Clone();
            tree.nodes = new List<Node>();
            
            Traverse(tree.RootNode, node =>
            {
                tree.nodes.Add(node);
            });
            
            return tree;
        }
    }
}
