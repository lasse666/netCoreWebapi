using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Gym.App.Common
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举的常数值(Decimal类型枚举)
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static decimal GetDecimal(this Enum em)
        {
            return Convert.ToDecimal(em);
        }
        //并发锁
        private static object locker = new object();
        //枚举键值对字典
        private static Dictionary<string, Dictionary<int, string>> listItems = new Dictionary<string, Dictionary<int, string>>();

        /// <summary>
        /// 初始化数据
        /// </summary>
        static EnumExtensions()
        {
            locker = new object();
            listItems = new Dictionary<string, Dictionary<int, string>>();
        }

        /// <summary>
        /// 获取枚举的常数值
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static int GetValue(this Enum em)
        {
            return Convert.ToInt32(em);
        }

        /// <summary>
        /// 获取枚举的System.ComponentModel.Description描述
        /// </summary>
        /// <param name="em"></param>
        /// <param name="defaultValue">无匹配时的默认返回值</param>
        /// <returns></returns>
        public static string GetDescription(this Enum em, string defaultValue = "")
        {
            int emValue = GetValue(em);
            Dictionary<int, string> emDictionary = ToDictionary(em);
            if (emDictionary.ContainsKey(emValue))
                return emDictionary[emValue];
            else
                return defaultValue;
        }

        /// <summary>
        /// 获取枚举的System.ComponentModel.Description描述
        /// </summary>
        /// <param name="em">要获取的枚举项的常数值</param>
        /// <param name="value">无匹配时的默认返回值</param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum em, int value, string defaultValue = "")
        {
            Dictionary<int, string> emDictionary = ToDictionary(em);
            if (emDictionary.ContainsKey(value))
                return emDictionary[value];
            else
                return defaultValue;
        }

        /// <summary>
        /// 判断常数值是否属于已申明枚举
        /// </summary>
        /// <param name="em"></param>
        /// <param name="value">要判断的常数值</param>
        /// <returns></returns>
        public static bool ContainsKey(this Enum em, int value)
        {
            Dictionary<int, string> emDictionary = ToDictionary(em);
            return emDictionary.ContainsKey(value);
        }

        /// <summary>
        /// 将枚举常数的名称转换成等效的枚举对象(绝对牛B的方法,却发现其实不怎么用,保留)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="em"></param>
        /// <param name="emString"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Get<T>(this Enum em, string emString, T defaultValue) where T : struct
        {
            Type tType = em.GetType();
            T result;
            if (Enum.TryParse<T>(emString, out result))
            {
                return result;
            }
            else
            {
                return defaultValue;
            }

        }

        /// <summary>
        /// 获取枚举成员的常数值和System.ComponentModel.Description描述的键值对
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static Dictionary<int, string> ToDictionary(this Enum em)
        {
            Dictionary<int, string> returnValue = new Dictionary<int, string>();
            Type emType = em.GetType();
            string emKey = emType.GUID.ToString("N");
            if (!listItems.ContainsKey(emKey))
            {
                lock (locker)
                {
                    if (!listItems.ContainsKey(emKey))
                    {
                        FieldInfo[] fieldInfos = emType.GetFields();
                        Dictionary<int, string> listCurrent = new Dictionary<int, string>();
                        foreach (FieldInfo fieldInfo in fieldInfos)
                        {
                            if (fieldInfo.FieldType.IsEnum)
                            {
                                DescriptionAttribute description = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
                                if (null != description)
                                {
                                    string strValue = fieldInfo.GetRawConstantValue().ToString();
                                    int key = Convert.ToInt32(strValue);
                                    listCurrent[key] = description.Description;
                                }
                            }
                        }
                        listItems[emKey] = listCurrent;
                    }
                }
            }
            return listItems[emKey];
        }
    }
}
