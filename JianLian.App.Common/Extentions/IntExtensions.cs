using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gym.App.Common
{
    /// <summary>
    /// System.Int 扩展
    /// Author:
    /// Version:1.0
    /// CreDate:
    public static class IntExtensions
    {

        /// <summary>
        /// ASCIIDecode
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ASCIIDecode(this int ASCIICode)
        {
            if (ASCIICode >= 0 && ASCIICode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)ASCIICode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
            {
                throw new Exception("ASCII Code is not valid.");
            }
        }

        /// <summary>
        /// 与计算
        /// </summary>
        /// <param name="value"></param>
        /// <param name="input"></param>
        /// <param name="includeZero"></param>
        /// <returns></returns>
        public static bool Anding(this int value, int input, bool includeZero = false)
        {
            if (!includeZero && value == 0)
                return false;
            else
                return (input & value) == input;
        }
    }
}
