using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.App.Common
{
  public  static class DataConversionHelper
    {
        /// <summary>
        /// 转换数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetConversionData(decimal data)
        {

            if (Math.Abs(data) < 10000)
            {
                return (data).ToString();
            }
            else
            {
                return Math.Round(data / 10000, 2).ToString();
            }

        }


    }
}
