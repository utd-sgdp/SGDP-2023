using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class ListExtensions
    {
        /// <summary>
        /// Creates and returns a shallow-copy of <see cref="list"/>.
        /// </summary>
        public static List<T> Clone<T>(this List<T> list)
        {
            List<T> result = new();

            foreach (var item in list)
            {
                result.Add(item);
            }
            
            return result;
        }
    }
}
