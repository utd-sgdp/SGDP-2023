using System;
using UnityEngine;

namespace Game.Agent.Tree
{
    public class AIAgent : MonoBehaviour
    {
        public BehaviourTree Tree;
        
        void Start()
        {
            if (Tree == null)
            {
                Debug.LogError($"No BehaviourTree was provided to { name }.");
                this.enabled = false;
                return;
            }

            // case: use BehaviourTree ScriptableObject
            if (GetType() == typeof(AIAgent))
            {
                // create runtime tree
                Tree = Tree.Clone();
            }
            // case: create BehaviourTree at runtime though code 
            else
            {
                Tree = CreateTree();
                if (Tree == null)
                {
                    Debug.LogError($"{ GetType().BaseType } must implement a { typeof(BehaviourTree) } in CreateTree().");
                    this.enabled = false;
                    return;
                }
            }
            
            Tree.Bind(gameObject, transform);
        }

        void Update()
        {
            State treeState = Tree.Update();
            if (treeState is State.Running) return;

            // root node finished execution
            // stop execution
            this.enabled = false;
        }

        /// <summary>
        /// Should be overriden to implement <see cref="BehaviourTree"/>'s in code. This will be ignored if
        /// <see cref="Tree"/> has already been assigned.
        /// </summary>
        /// <returns> The <see cref="BehaviourTree"/> to be used by this <see cref="AIAgent"/>. </returns>
        protected virtual BehaviourTree CreateTree() => null;

        #if UNITY_EDITOR
        [Button(Spacing = 15)]
        public void ValidateDependencies()
        {
            bool isValid = true;
            BehaviourTree.Traverse(Tree.RootNode, node =>
            {
                foreach (string type in node.GetDependencies())
                {
                    Type t = Type.GetType(type);
                    if (t == null)
                    {
                        Debug.LogWarning($"'{ node.name }' has unknown dependency type: { type }.");
                        continue;
                    }

                    var component = gameObject.GetComponentInChildren(t);
                    if (component)
                    {
                        continue;
                    }

                    Debug.LogError($"'{ gameObject.name }' is missing a component: { type }.");
                    isValid = false;
                }
            });

            if (isValid)
            {
                Debug.Log("SUCCESS: this object has all known dependencies.");
            }
        }
        #endif
    }
}
