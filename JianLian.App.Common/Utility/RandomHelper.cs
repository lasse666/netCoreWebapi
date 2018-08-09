using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.App.Common
{
    public static class RandomHelper
    {
        /// <summary>
		/// 随机数生成器
		/// </summary>
        public static Random Generator { get; } = new Random(SystemRandomInt());

        /// <summary>
        /// 使用RNGCryptoServiceProvider生成真正随机的二进制数据
        /// </summary>
        public static byte[] SystemRandomBytes()
        {
            byte[] bytes = System.Guid.NewGuid().ToByteArray();
            return bytes;
        }

        /// <summary>
        /// 使用RNGCryptoServiceProvider生成真正随机的整数
        /// </summary>
        public static int SystemRandomInt()
        {
            return BitConverter.ToInt32(SystemRandomBytes(), 0);
        }

        /// <summary>
        /// 生成10位数的编号
        /// </summary>
        public static string GenerateSerial()
        {
            //把guid转换为两个long int
            byte[] bytes = System.Guid.NewGuid().ToByteArray();
            UInt64 high = System.BitConverter.ToUInt64(bytes, 0);
            UInt64 low = System.BitConverter.ToUInt64(bytes, 8);
            //转换为10位数字
            UInt64 mixed = ((high ^ low) % 8999999999) + 1000000000;
            return mixed.ToString();
        }

        private static object lockobject=new object();
        public static string GenerateOrderId()
        {
            lock (lockobject)
            {
                long id = DateTime.Now.Ticks - DateTime.Parse("2017/01/01").Ticks;
                return id.ToString("D10");

            }
        }


        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <param name="chars">包含的字符，默认是a-zA-Z0-9</param>
        /// <returns></returns>

        public static string RandomString(int length, string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            var buffer = new char[length];
            for (int n = 0; n < length; ++n)
            {
                buffer[n] = chars[Generator.Next(chars.Length)];
            }
            return new string(buffer);
        }

        /// <summary>
        ///生成随机数字字符串
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static string RandomNumber(int length, string chars = "0123456789")
        {
            var buffer = new char[length];
            for (int n = 0; n < length; ++n)
            {
                buffer[n] = chars[Generator.Next(chars.Length)];
            }
            return new string(buffer);
        }
    }
}
