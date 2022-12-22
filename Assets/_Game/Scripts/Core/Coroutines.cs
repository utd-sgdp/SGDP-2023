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
        
        #region Promise-Dependent
        public static IEnumerator Wait(Promise promise, float duration, bool inRealTime = false)
        {
            if (inRealTime) yield return new WaitForSecondsRealtime(duration);
            else yield return new WaitForSeconds(duration);
            
            promise.Fulfill();
        }

        public static IEnumerator WaitAndProgress(Promise promise, float duration, bool inRealTime = false)
        {
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                promise.Progress(elapsedTime / duration);
                
                if (inRealTime) yield return new WaitForSecondsRealtime(duration);
                else yield return new WaitForSeconds(duration);

                elapsedTime += inRealTime ? Time.unscaledDeltaTime : Time.deltaTime;
            }
            
            promise.Progress(1);
            promise.Fulfill();
        }

        public static IEnumerator Lerp(Promise promise, float from, float to, float maxDelta)
        {
            float current = from;

            while (Mathf.Abs(current - to) > float.Epsilon)
            {
                current = Mathf.MoveTowards(current, to, maxDelta);
                promise.Progress(current);
                
                yield return null;
            }
            
            promise.Progress(to);
            promise.Fulfill();
        }
        #endregion
    }
}