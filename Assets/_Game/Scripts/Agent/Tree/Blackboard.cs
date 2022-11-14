using System;
using UnityEngine;

namespace Game.Agent.Tree
{
    [System.Serializable]
    public class Blackboard
    {
        public Transform target;

        public GameObject gameObject { get; private set; }
        public Transform transform { get; private set; }

        /// <summary>
        /// Initialize required information/references collected at runtime.
        /// </summary>
        public void RuntimeInitialize(GameObject go, Transform trans)
        {
            gameObject = go;
            transform = trans;
        }
    }
}