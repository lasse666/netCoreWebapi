using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.App.Common
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Take<T>(this IQueryable<T> sources, int size, int page, out int count) where T : class
        {
            count = sources.Count();
            int skip = size * (page - 1);
            if (skip < 0) skip = 0;
            return sources.Skip(skip).Take(size);
        }

        public static IEnumerable<T> Take<T>(this IEnumerable<T> sources, int size, int page, out int count) where T : class
        {
            count = sources.Count();
            int skip = size * (page - 1);
            if (skip < 0) skip = 0;
            return sources.Skip(skip).Take(size);
        }

    }
}
