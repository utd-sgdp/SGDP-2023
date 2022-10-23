using UnityEngine;

namespace Game.Agent.Tree
{
    public class AIAgent : MonoBehaviour
    {
        [SerializeField]
        public BehaviourTree _tree;
        
        void Start()
        {
            if(_tree != null)
            {
                //Replaces tree with a clone of the tree to prevent multiple AIAgents using the same tree from conflicting.
                _tree = _tree.Clone();
            }
            // allow sub-classes to create a tree through code
            if (_tree == null)
            {
                _tree = CreateTree();
                if (_tree == null)
                {
                    Debug.LogError($"{ GetType().BaseType } must implement a { typeof(BehaviourTree) } in CreateTree().");
                    this.enabled = false;
                    return;
                }
            }
            
            _tree.Bind(gameObject, transform);
        }

        void Update()
        {
            State treeState = _tree.Update();
            if (treeState is State.Running) return;

            // root node finished execution
            // stop execution
            this.enabled = false;
        }

        /// <summary>
        /// Should be overriden to implement <see cref="BehaviourTree"/>'s in code. This will be ignored if
        /// <see cref="_tree"/> has already been assigned.
        /// </summary>
        /// <returns> The <see cref="BehaviourTree"/> to be used by this <see cref="AIAgent"/>. </returns>
        protected virtual BehaviourTree CreateTree() => null;
    }
}
