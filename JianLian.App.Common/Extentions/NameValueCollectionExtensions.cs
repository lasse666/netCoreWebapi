using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace Gym.App.Common
{
    /// <summary>
    /// System.Collections.Specialized.NameValueCollection数据获取扩展
    /// Author:
    /// Version:2.0
    /// CreDate:
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// 获取集合中具有指定键的项的String类型值
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key">要定位的项的System.String键。</param>
        /// <param name="defaultVaule">无匹配或转换类型失败时的默认返回值</param>
        /// <returns></returns>
        public static string GetString(this NameValueCollection collection, string key, string defaultVaule = "")
        {
            return null == collection[key] ? defaultVaule : collection[key];
        }

        public static string[] GetStringArray(this NameValueCollection collection, string key, string separator = ",")
        {
            List<string> values = new List<string>();
            string input = collection[key];
            if (!string.IsNullOrEmpty(input))
            {
                foreach (string strKey in input.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries))
                {
                    values.Add(strKey);
                }
            }
            return values.ToArray();
        }


        public static int[] GetIntArray(this NameValueCollection collection, string key, string separator = ",")
        {
            List<int> values = new List<int>();
            string input = collection[key];
            if (!string.IsNullOrEmpty(input))
            {
                foreach (string strKey in input.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int value = 0;
                    if (int.TryParse(strKey, out value))
                        values.Add(value);
                }
            }
            return values.ToArray();
        }

        public static Guid[] GetGuidArray(this NameValueCollection collection, string key, string separator = ",")
        {
            List<Guid> values = new List<Guid>();
            string input = collection[key];
            if (!string.IsNullOrEmpty(input))
            {
                foreach (string strKey in input.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries))
                {
                    Guid value;
                    if (Guid.TryParse(strKey, out value))
                        values.Add(value);
                }
            }
            return values.ToArray();
        }

        /// <summary>
        /// 获取集合中具有指定键的项的Int类型值
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key">要定位的项的System.String键。</param>
        /// <param name="defaultValue">无匹配或转换类型失败时的默认返回值</param>
        /// <returns></returns>
        public static int GetInt(this NameValueCollection collection, string key, int defaultValue = 0)
        {
            int returnValue;
            return int.TryParse(collection[key], out returnValue) ? returnValue : defaultValue;
        }

        /// <summary>
        /// 获取集合中具有指定键的项的DateTime类型值
        /// 注意,此方法为兼容老版本数据库DateTime(MSSQL2005-,MySql等)类型 默认DateTime.MinValue为1900-01-01 若需设置0000-01-01 请将minValue参数设置为true
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key">要定位的项的System.String键。</param>
        /// <param name="defaultValue">无匹配或转换类型失败时的默认返回值</param>
        /// <param name="minValue">是否开启DateTime.MinValue</param>
        /// <returns></returns>
        public static DateTime GetDateTime(this NameValueCollection collection, string key, DateTime defaultValue = default(DateTime), bool minValue = false)
        {
            if (defaultValue == DateTime.MinValue && !minValue)
                defaultValue = FixedVariable.MinDate;
            DateTime returnValue;
            return DateTime.TryParse(collection[key], out returnValue) ? returnValue : defaultValue;
        }

        /// <summary>
        /// 获取集合中具有指定键的项的Decimal类型值
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key">要定位的项的System.String键。</param>
        /// <param name="defaultValue">无匹配或转换类型失败时的默认返回值</param>
        /// <param name="decimals">保留小数位</param>
        /// <returns></returns>
        public static decimal GetDecimal(this NameValueCollection collection, string key, decimal defaultValue = 0.00m, int decimals = 2)
        {
            decimal returnValue;
            return Math.Round(decimal.TryParse(collection[key], out returnValue) ? returnValue : defaultValue, decimals);
        }

        /// <summary>
        /// 获取集合中具有指定键的项的Float类型值
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key">要定位的项的System.String键。</param>
        /// <param name="defaultValue">无匹配或转换类型失败时的默认返回值</param>
        /// <returns></returns>
        public static float GetFloat(this NameValueCollection collection, string key, float defaultValue = 0.0f)
        {
            float returnValue;
            return float.TryParse(collection[key], out returnValue) ? returnValue : defaultValue;
        }

        /// <summary>
        /// 获取集合中具有指定键的项的Double类型值
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key">要定位的项的System.String键。</param>
        /// <param name="defaultValue">无匹配或转换类型失败时的默认返回值</param>
        /// <returns></returns>
        public static double GetDouble(this NameValueCollection collection, string key, double defaultValue = 0.0)
        {
            double returnValue;
            return double.TryParse(collection[key], out returnValue) ? returnValue : defaultValue;
        }

        /// <summary>
        /// 获取集合中具有指定键的项的Boolean类型值
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key">要定位的项的System.String键。</param>
        /// <param name="defaultValue">无匹配或转换类型失败时的默认返回值</param>
        /// <returns></returns>
        public static bool GetBoolean(this NameValueCollection collection, string key, bool defaultValue = false)
        {
            bool returnValue;
            return bool.TryParse(collection[key], out returnValue) ? returnValue : defaultValue;
        }


        /// <summary>
        /// 获取集合中具有指定键的项的T类型值
        /// </summary>
        /// <typeparam name="T">要获取的数据类型,必须是基本类型</typeparam>
        /// <param name="collection"></param>
        /// <param name="key">要定位的项的System.String键。</param>
        /// <param name="defaultValue">无匹配或转换类型失败时的默认返回值</param>
        /// <returns></returns>
        public static T Get<T>(this NameValueCollection collection, string key, T defaultValue)
        {
            T returnValue = defaultValue;
            if (null != collection[key])
            {
                Type tType = typeof(T);
                if (tType.IsGenericType && tType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return (T)Convert.ChangeType(collection[key], Nullable.GetUnderlyingType(tType));
                }
                else if (tType.IsEnum)
                {
                    try
                    {
                        return (T)Enum.Parse(tType, collection[key].ToString());
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
                else
                {
                    try
                    {
                        return (T)Convert.ChangeType(collection[key], tType);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 获取集合中具有指定键的项的T类型值(注：T类型应与defaultValue类型相同)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetEnum<T>(this NameValueCollection collection, string key, Enum defaultValue) where T : struct
        {
            Type tType = typeof(T);
            //判断类型条件是否成立
            if (tType != defaultValue.GetType())
                throw new Exception("T must be the same type with param defaultValue.");
            int emValue = GetInt(collection, key, defaultValue.GetValue());
            if (defaultValue.ContainsKey(emValue))
                return (T)Enum.Parse(tType, emValue.ToString());
            else
                return (T)Convert.ChangeType(defaultValue, tType);

        }

        /// <summary>
        /// 确定集合中指向键的项是否与指定System.Int对象有相同值
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key">要定位的项的System.String键。</param>
        /// <param name="value">要与此指定项对比的System.Int类型值</param>
        /// <returns></returns>
        public static bool SameAs(this NameValueCollection collection, string key, int value)
        {
            return collection.GetInt(key, 0) == value;
        }

        /// <summary>
        /// 确定集合中指向键的项是否与指定System.String对象有相同值
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="key">要定位的项的System.String键。</param>
        /// <param name="value">要与此指定项对比的System.String类型值。</param>
        /// <param name="ignoreCase">是否区分大小写</param>
        /// <returns></returns>
        public static bool SameAs(this NameValueCollection collection, string key, string value, bool ignoreCase = false)
        {
            if (collection[key] == value)
                return true;
            else if (null == collection[key])
                return collection[key] == value;
            else
                return collection[key].ToLower() == value.ToLower();
        }

        public static Guid GetGuid(this NameValueCollection collection, string key)
        {
            Guid returnValue;
            if (Guid.TryParse(collection[key], out returnValue))
                return returnValue;
            else
                return Guid.Empty;
        }

        ///// <summary>
        ///// 获取集合中指定键的项的Int类型值的和
        ///// </summary>
        ///// <param name="collection"></param>
        ///// <param name="key"></param>
        ///// <param name="defaultValue"></param>
        ///// <returns></returns>
        //public static int GetSum(this NameValueCollection collection, string key, int defaultValue = 0)
        //{
        //    int returnValue = 0;
        //    string sumString = collection[key];
        //    if (!string.IsNullOrEmpty(sumString) && Regex.IsMatch(sumString, RegexPattern.IdString))
        //    {
        //        foreach (string valueString in sumString.Split(','))
        //        {
        //            int value = Converter.ToInt32(valueString);
        //            returnValue += value;
        //        }
        //    }
        //    else
        //        returnValue = defaultValue;
        //    return returnValue;

        //}

        public static int Sum(this NameValueCollection collection, string key)
        {
            int sum = 0;
            int[] values = GetIntArray(collection, key);
            foreach (var value in values)
            {
                sum += value;
            }
            return sum;
        }


    }
}
