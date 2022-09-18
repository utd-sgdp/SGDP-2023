using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utility.Patterns
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance => _instance;
        static T _instance;

        protected virtual void Awake()
        {
            // enforce single instance rule
            if (!_instance)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (_instance != this) return;

            _instance = null;
        }
    }
}