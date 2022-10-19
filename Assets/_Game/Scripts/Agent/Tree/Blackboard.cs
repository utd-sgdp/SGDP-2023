using System;
using UnityEngine;

namespace Game.Agent.Tree
{
    [System.Serializable]
    public class Blackboard
    {
        [NonSerialized] public GameObject gameObject;
        [NonSerialized] public Transform transform;

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