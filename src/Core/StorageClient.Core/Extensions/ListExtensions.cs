using System.Collections.Generic;

namespace StorageClient.Core.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        ///     Get first element and remove from list
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="list">List of objects</param>
        /// <returns>First element from list</returns>
        public static T PopFirst<T>(this IList<T> list)
        {
            var firstElement = list[0];
            list.RemoveAt(0);
            return firstElement;
        }

        /// <summary>
        ///     Get last element and remove from list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns>Last element from list</returns>
        public static T PopLast<T>(this IList<T> list)
        {
            var firstElement = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return firstElement;
        }
    }
}