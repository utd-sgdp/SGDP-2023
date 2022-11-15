using System.Collections;
using System.Collections.Generic;

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

    public static class StackExtensions
    {
        public static void Push<T>(this Stack<T> stack, IEnumerable<T> data)
        {
            foreach (var item in data)
            {
                stack.Push(item);
            }
        }
    }
}
