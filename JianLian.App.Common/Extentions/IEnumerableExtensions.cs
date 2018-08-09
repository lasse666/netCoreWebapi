using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.App.Common
{
    /// <summary>
    /// 集合的扩展类
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 根据条件去除重复项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <returns></returns>
        public static IEnumerable<T> Distinct<T, V>(this IEnumerable<T> source, Func<T, V> keySelector)
        {
            return source.Distinct(new CommonEqualityComparer<T, V>(keySelector));
        }
    }

    public class CommonEqualityComparer<T, V> : IEqualityComparer<T>
    {
        private Func<T, V> keySelector;

        public CommonEqualityComparer(Func<T, V> keySelector)
        {
            this.keySelector = keySelector;
        }

        public bool Equals(T x, T y)
        {
            return EqualityComparer<V>.Default.Equals(keySelector(x), keySelector(y));
        }

        public int GetHashCode(T obj)
        {
            return EqualityComparer<V>.Default.GetHashCode(keySelector(obj));
        }
    }
}
