using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.App.Common
{
    public static class GuidExtensions
    {
        public static bool IsEmpty(this Guid guid)
        {
            return guid.Equals(Guid.Empty);
        }
        public static bool IsNotEmpty(this Guid guid)
        {
            return !guid.Equals(Guid.Empty);
        }

        public static string ToUrlString(this Guid guid)
        {
            return guid.ToString("N");
        }


    }
}
