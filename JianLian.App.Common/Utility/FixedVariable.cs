using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.App.Common
{
    public class FixedVariable
    {
        public static DateTime MinDate
        {
            get { return new DateTime(1900, 1, 1); }
        }
    }
}
