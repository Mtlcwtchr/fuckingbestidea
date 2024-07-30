using System.Collections.Generic;

namespace Core.Utilities
{
    public static class ListUtils
    {
        public static T GetRandomElement<T>(this IList<T> list, int seed = 0)
        {
            if (list.Count < 1)
                return default(T);

            var index = seed == 0 ? UnityEngine.Random.Range(0, list.Count) : MathUtils.Range(0, list.Count, seed);
            return list[index];
        }

        public static void RemoveFirst<T>(this List<T> list)
        {
            if (list.Count < 1)
                return;
            
            list.RemoveAt(0);
        }

        public static void RemoveLast<T>(this List<T> list)
        {
            if (list.Count < 1)
                return;

            list.RemoveAt(list.Count - 1);
        }

        public static T PopFront<T>(this List<T> list)
        {
            if (list.Count < 1)
                return default(T);
            
            var obj = list[0];
            list.RemoveAt(0);
            return obj;
        }

        public static T PopBack<T>(this List<T> list)
        {
            if (list.Count < 1)
                return default(T);
            
            var obj = list[^1];
            list.RemoveAt(list.Count - 1);
            return obj;
        }
    }
}