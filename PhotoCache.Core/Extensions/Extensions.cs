using System.Collections.Generic;

namespace PhotoCache.Core.Extensions
{
    public static class Extensions
    {
        public static void AddOrUpdate<T1, T2>(this Dictionary<T1, T2> dict, T1 key, T2 value)
        {
            if (dict.ContainsKey(key))
                dict[key] = value;
            else
                dict.Add(key, value);
        }
    }
}
