using UnityEngine;

namespace Game.Utility.Patterns
{
    public class LazySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<T>();
                    if (!_instance)
                    {
                        throw new System.NullReferenceException($"{typeof(T).Name} has no instance in the scene.");
                    }
                }

                return _instance;
            }
        }
        static T _instance;

        protected virtual void OnDestroy()
        {
            if (_instance != this) return;

            _instance = null;
        }
    }
}