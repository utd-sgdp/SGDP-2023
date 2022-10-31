using UnityEngine;

namespace Game.Agent.Tree
{
    [CreateAssetMenu]
    public class BehaviourTree : ScriptableObject
    {
        public Node RootNode;
        public State TreeState = State.Running;
        public Blackboard Blackboard = new Blackboard();

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
    }
}
