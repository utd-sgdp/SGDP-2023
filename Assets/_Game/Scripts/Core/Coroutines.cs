using System.Collections;
using Game.Utility;
using UnityEngine;

namespace Game
{
    public static class Coroutines
    {
        public static Empty Dummy
        {
            get
            {
                if (!dummy)
                {
                    GameObject go = new GameObject("Dummy GameObject for Coroutines"); 
                    Object.DontDestroyOnLoad(go);
                    dummy = go.AddComponent<Empty>();
                }

                return dummy;
            }
        }
        static Empty dummy;
        
        public static IEnumerator WaitThen(float duration, System.Action callback)
        {
            yield return new WaitForSeconds(duration);
            callback?.Invoke();
        }

        public static IEnumerator WaitFrame(System.Action callback) => WaitFrames(1, callback);
        public static IEnumerator WaitFrames(int duration, System.Action callback)
        {
            int i = 0;
            while (i++ < duration) yield return null;
            
            callback?.Invoke();
        }
    }
}