using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Gym.App.Common
{
    /// <summary>
    /// System.String 扩展
    /// Author:
    /// Version:2.0
    /// CreDate:
    /// </summary>
    public static class StringExtensions
    {
        private static readonly string PCManagerSite = ConfigurationManager.AppSettings["PCManagerSite"].ToString();
        /// <summary>
        /// 正则验证
        /// </summary>
        /// <param name="str"></param>
        /// <param name="patter"></param>
        /// <param name="allowNullOrEmpty"></param>
        /// <returns></returns>
        public static bool IsMatch(this string str, string patter, bool allowNullOrEmpty = true)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return Regex.IsMatch(str, patter);
            }
            else
                return allowNullOrEmpty;
        }

        ///// <summary>
        ///// 将字符串转换成int类型的List集合
        ///// </summary>
        ///// <param name="str"></param>
        ///// <param name="separator"></param>
        ///// <returns></returns>
        //public static List<int> ConverToIntList(this string str, string separator = ",")
        //{
        //    List<int> listReturns = new List<int>();
        //    string patter = "^[-]?\\d+(" + separator + "[-]?\\d+)*$";
        //    if (str.IsMatch(patter, false))
        //    {
        //        foreach (string key in str.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            int value = Converter.ToInt32(key);
        //            listReturns.Add(value);
        //        }
        //    }
        //    return listReturns;
        //}

        /// <summary>
        /// HtmlEncode
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string str)
        {
            return System.Web.HttpUtility.HtmlEncode(str.Trim());
        }

        /// <summary>
        /// HtmlDeCode
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string HtmlDecode(this string str)
        {
            return System.Web.HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// UrlEncode
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(this string str)
        {
            return System.Web.HttpUtility.UrlEncode(str.Trim());
        }

        /// <summary>
        /// UrlDecode
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlDecode(this string str)
        {
            return System.Web.HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// ASCIIEncode
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ASCIIEncode(this string str)
        {
            if (str.Length == 1)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCode = (int)asciiEncoding.GetBytes(str)[0];
                return (intAsciiCode);
            }
            else
            {
                throw new Exception("Character is not valid.");
            }
        }
        /// <summary>
        /// 根据字节数截取字符串
        /// </summary>
        /// <param name="s">输入的字符串</param>
        /// <param name="length">要截取的字节长度</param>
        /// <returns></returns>
        public static string bSubstring(this string s, int length)
        {
            if (string.IsNullOrEmpty(s))
            {
                return "";
            }
            if (length >= s.GetStringByteLength())
            {
                return s;
            }
            string outStr = "";
            int j = 0;
            foreach (char c in s)
            {
                string checkString = c + "";
                j += Encoding.Default.GetByteCount(checkString);
                outStr += c;
                if (j >= length) break;
            }
            return outStr + "...";
        }

        /// <summary>
        /// 获取字符串的字节长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetStringByteLength(this string str)
        {
            int j = 0;
            foreach (char c in str)
            {
                string checkString = c + "";
                j += Encoding.Default.GetByteCount(checkString);
            }
            return j;
        }

        /// <summary>
        /// 将字符串转换成Guid,当转换失败时返回Guid.Empty
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Guid AsGuid(this string str)
        {
            Guid guid;
            if (Guid.TryParse(str, out guid))
                return guid;
            else
                return Guid.Empty;
        }

        /// <summary>
        /// 将字符串转换成Int
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int AsInt(this string str, int defaultValue = 0)
        {
            int returnValue = 0;
            if (int.TryParse(str, out returnValue))
                return returnValue;
            else
                return defaultValue;
        }

        public static string ToMd5(this string str)
        {
            byte[] data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sb.AppendFormat("{0:x2}", data[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取图片完整路径
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string InsertPCSitePrefix(this string str, char separator = '|')
        {
            if (!string.IsNullOrWhiteSpace(str))
            {
                List<string> result = new List<string>();
                List<string> list = str.Split(separator).ToList();
                //added by plx on 20180626
                foreach (var item in list)
                {
                    ////判断各个项 是否为guid
                    //Guid itemGuid = Guid.Empty;
                    ////文件路径保存的是个GUID
                    //if (Guid.TryParse(item, out itemGuid))
                    //{
                    //    result.Add(FilesWebapiPath + string.Format(@"api/Default/GetImg?ImgKey={0}", itemGuid.ToString()));
                    //}
                    //else
                    //{
                    if (!item.Contains("http"))
                    {
                        result.Add(PCManagerSite + item);
                    }
                    else
                    {
                        result.Add(item);
                    }
                    //}
                }
                return string.Join("|", result);
            }
            return str;
        }

        /// <summary>
        /// 插入本地前缀
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string InsertLocalSitePrefix(this string str)
        {
            string localSite = string.Empty;//HttpContext.Current.Request.Url.Scheme + @"://" + HttpContext.Current.Request.Url.Authority; TODO.

            if (!string.IsNullOrWhiteSpace(str))
            {
                List<string> result = new List<string>();
                List<string> list = str.Split('|').ToList();
                foreach (var item in list)
                {
                    if (!item.Contains("http"))
                    {
                        result.Add(localSite + item);
                    }
                    else
                    {
                        result.Add(item);
                    }
                }
                return string.Join("|", result);
            }
            return str;
        }

        public static string InsertLocalSitePrefix(this string str,Uri uri)
        {
            string localSite = uri.Scheme + @"://" + uri.Authority;

            if (!string.IsNullOrWhiteSpace(str))
            {
                List<string> result = new List<string>();
                List<string> list = str.Split('|').ToList();
                foreach (var item in list)
                {
                    if (!item.Contains("http"))
                    {
                        result.Add(localSite + item);
                    }
                    else
                    {
                        result.Add(item);
                    }
                }
                return string.Join("|", result);
            }
            return str;
        }


        public static List<int> SplitToIntList(this string str, char separator)
        {
            List<int> resultList = new List<int>();
            if (!string.IsNullOrWhiteSpace(str))
            {
                var list = str.Split(separator);
                foreach (var item in list)
                {
                    int temp;
                    if (int.TryParse(item, out temp))
                    {
                        resultList.Add(temp);
                    }
                }
            }        
            return resultList;
        }

        public static string BoldValue(this string value)
        {
            return "<b>" + value + "</b>";
        }

    }
}
