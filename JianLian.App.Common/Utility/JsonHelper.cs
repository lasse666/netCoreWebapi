using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.App.Common
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public class JsonHelper
    {
        public static string ObjectToString<T>(T obj) where T : new()
        {
            return JsonConvert.SerializeObject(obj);           
        }

        public static T StringToObject<T>(string content) where T : new()
        {
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static T FileToObject<T>(string fullPathName) where T : new()
        {
            if (string.IsNullOrEmpty(fullPathName))
                throw new ArgumentNullException("fullPathName");

            if (File.Exists(fullPathName))
            {
                return StringToObject<T>(File.ReadAllText(fullPathName, Encoding.Default));
            }
            else
            {
                return default(T);
            }
        }

        public static bool ObjectToFile<T>(T obj, string fullPathName) where T : new()
        {
            if (string.IsNullOrEmpty(fullPathName))
                throw new ArgumentNullException("fullPathName");

            try
            {
                string fullPath = Path.GetDirectoryName(fullPathName);
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                using (FileStream fileStream = File.Create(fullPathName))
                {
                    string text = JsonConvert.SerializeObject(obj);
                    byte[] bytes = Encoding.Default.GetBytes(text);
                    fileStream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }




        /// <summary>
        /// 将Json字符串转换为对象
        /// </summary>
        /// <param name="json">Json字符串</param>
        public static T ToObject<T>(string json)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                    return default(T);
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception)
            {
                return default(T); 
            }
        }

        /// <summary>
        /// 将对象转换为Json字符串
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="isConvertSingleQuotes">是否将双引号转成单引号</param>
        public static string ToJson(object target, bool isConvertSingleQuotes = false)
        {
            if (target == null)
                return "{}";
            var result = JsonConvert.SerializeObject(target);
            if (isConvertSingleQuotes)
                result = result.Replace("\"", "'");
            return result;
        }

        ///// <summary>
        ///// 将对象转换为Json字符串，并且去除两侧括号
        ///// </summary>
        ///// <param name="target">目标对象</param>
        ///// <param name="isConvertSingleQuotes">是否将双引号转成单引号</param>
        //public static string ToJsonWithoutBrackets(object target, bool isConvertSingleQuotes = false)
        //{
        //    var result = ToJson(target, isConvertSingleQuotes);
        //    if (result == "{}")
        //        return result;
        //    return result.TrimStart('{').TrimEnd('}');
        //}

        //public static T GetValue<T>(this JToken jToken, string key, T defaultValue = default(T))
        //{
        //    dynamic ret = jToken[key];
        //    if (ret == null) return defaultValue;
        //    if (ret is JObject) return JsonConvert.DeserializeObject<T>(ret.ToString());
        //    return (T)ret;
        //}


    }
}
