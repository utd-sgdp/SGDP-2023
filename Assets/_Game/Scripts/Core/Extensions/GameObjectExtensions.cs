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

    public static class TransformExtensions
    {
        public static void SetWorldScale(this Transform transform, Vector3 value)
        {
            Vector3 factor = Vector3.one;
            Transform ancestor = transform.parent;
            while (ancestor != null)
            {
                Vector3 local = ancestor.localScale;
                local.x = 1 / local.x;
                local.y = 1 / local.y;
                local.z = 1 / local.z;
                
                factor.Scale(local);
                ancestor = ancestor.parent;
            }
            
            value.Scale(factor);
            transform.localScale = value;
        }
    }
}