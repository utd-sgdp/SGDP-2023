using System.Collections;
using UnityEngine;

namespace Game
{
    public static class Coroutines
    {
        public static IEnumerator WaitThen(float duration, System.Action callback)
        {
            yield return new WaitForSeconds(duration);
            callback?.Invoke();
        }
    }
}