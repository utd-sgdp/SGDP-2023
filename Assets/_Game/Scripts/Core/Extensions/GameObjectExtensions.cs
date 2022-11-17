using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public static class GameObjectExtensions
    {
        public static void Traverse(this List<GameObject> objects, System.Action<GameObject> callback)
        {
            while (objects.Count > 0)
            {
                // get current gameobject
                GameObject go = objects[0];
                objects.RemoveAt(0);

                // add children to search list
                objects.AddRange(
                    from Transform child in go.transform
                    select child.gameObject
                );
                
                // perform callbacks
                callback?.Invoke(go);
            }
        }
    }
}