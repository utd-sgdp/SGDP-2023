using System;
using System.Collections;
using System.Collections.Generic;
using Game.Utility;
using UnityEngine;

namespace Game.Agent.Tree
{
    public enum State
    {
        Running = 0, Success = 1, Failure = 2,
    }
    
    public abstract class Node : ScriptableObject
    {
        public State CurrentState { get; protected set; } = State.Running;
        public bool Started { get; protected set; }
        
        [TextArea(3, 8)] public string Description;
        [HideInInspector] public string guid;
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

        /// <summary>
        /// Create deep copy of this <see cref="Node"/>. Useful to clone at runtime to prevent errors caused by
        /// multiple agents using the same tree.
        /// </summary>
        /// <returns></returns>
        public virtual Node Clone()
        {
            return Instantiate(this);
        }

        public virtual IReadOnlyCollection<string> GetDependencies() => new string[] { };

        #region MonoBehaviour Wrappers
        protected GameObject gameObject => Blackboard.gameObject;
        protected Transform transform => Blackboard.transform;
        
        protected static void print(object message) => Debug.Log(message);

        protected Coroutine StartCoroutine(IEnumerator enumerator) => Blackboard.aiAgent.StartCoroutine(enumerator);
        protected void StopCoroutine(Coroutine routine) => Blackboard.aiAgent.StopCoroutine(routine);
        protected void StopAllCoroutines() => Blackboard.aiAgent.StopAllCoroutines();
        
        protected T GetComponent<T>() => Blackboard.gameObject.GetComponent<T>();
        protected Component GetComponent(Type type) => Blackboard.gameObject.GetComponent(type);
        protected Component GetComponent(string type) => Blackboard.gameObject.GetComponent(type);
        
        protected T GetComponentInChildren<T>(bool includeInactive=false) => Blackboard.gameObject.GetComponentInChildren<T>(includeInactive);
        protected Component GetComponentInChildren(Type type, bool includeInactive=false) => Blackboard.gameObject.GetComponentInChildren(type, includeInactive);
        
        protected T[] GetComponentsInChildren<T>(bool includeInactive=false) => Blackboard.gameObject.GetComponentsInChildren<T>(includeInactive);
        protected Component[] GetComponentsInChildren(Type type, bool includeInactive=false) => Blackboard.gameObject.GetComponentsInChildren(type, includeInactive);
        #endregion
    }
}
