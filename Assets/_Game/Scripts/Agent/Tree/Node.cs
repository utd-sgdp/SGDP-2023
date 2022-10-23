using System.Collections.Generic;
using UnityEngine;

namespace Game.Agent.Tree
{
    public enum State
    {
        Running = 0, Success = 1, Failure = 2,
    }
    
    public abstract class Node : ScriptableObject
    {
        [HideInInspector] public State CurrentState { get; protected set; } = State.Running;
        [HideInInspector] public bool Started { get; protected set; }
        [HideInInspector] public string guid;
        [TextArea] public string Description;
        [HideInInspector] public Blackboard Blackboard;
        [HideInInspector] public Vector2 editorPosition;

        // ReSharper disable Unity.PerformanceAnalysis
        public State Update()
        {
            // attempt to start
            if (!Started)
            {
                OnStart();
                Started = true;
            }

            CurrentState = OnUpdate();

            // attempt to exit
            if (CurrentState is State.Failure or State.Success)
            {
                OnStop();
                Started = false;
            }
            
            return CurrentState;
        }

        /// <summary>
        /// Called on entering a node. More specifically, right before <see cref="OnUpdate"/> (on the same frame).
        /// </summary>
        protected abstract void OnStart();
        
        /// <summary>
        /// Called on exiting a node. More specifically, right after <see cref="OnUpdate"/> (on the same frame).
        /// </summary>
        protected abstract void OnStop();

        /// <summary>
        /// Called every frame while this <see cref="Node"/> is running.
        /// </summary>
        /// <returns>
        /// Sets this <see cref="Node"/>'s <see cref="CurrentState"/>. If not <see cref="State.Running"/>, 
        /// This node will exit.
        /// </returns>
        protected abstract State OnUpdate();

        public abstract List<Node> GetChildren();

        //Allow AIAgent to clone nodes to copy the behaviour tree for runtime usage to prevent multiple AIAgents using the same tree from conflicting.
        public virtual Tree.Node Clone()
        {
            return Instantiate(this);
        }
    }
}
