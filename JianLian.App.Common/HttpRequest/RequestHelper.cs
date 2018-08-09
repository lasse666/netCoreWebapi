using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Gym.App.Common
{
    public class RequestHelper
    {
        /// <summary>  
        /// 发送Get请求  
        /// </summary>  
        /// <param name="url">地址</param>  
        /// <param name="dicParams">请求参数定义</param>  
        /// <returns></returns>  
        public static string SendGetRequest(string url, Dictionary<string, string> dicParams)
        {
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dicParams!=null && dicParams.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in dicParams)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            req.Method = "GET";
            req.Timeout = 10000;
            req.ContentType = "text/html;charset=UTF-8";
            //req.Headers.Set("", "");
            //添加参数  
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容  
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
        }


        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="paramsJson">JSON格式的参数</param>
        /// <returns></returns>
        /// 参数格式："{  \"app_id\": \"757471044460048\", \"app_secret\": \"ODY5ZDcxOWYwM2M2ZTFjNTY1ZTI2ZmViYjJhZWIyYWZmNGQzMTk4NQ\"}"
        public static JObject PostRequest(string url, string paramsJson = null)
        {
            JObject result = new JObject();
            result["success"] = false;

            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/json";

                if (!string.IsNullOrEmpty(paramsJson))
                {
                    byte[] bs = Encoding.ASCII.GetBytes(paramsJson);    //参数转化为ascii码
                    request.ContentLength = bs.Length;
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bs, 0, bs.Length);
                    }
                }


                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
                    {
                        var htmlStr = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(htmlStr))
                        {
                            result["success"] = true;
                            result["data"] = JObject.Parse(htmlStr);
                        }
                    }
                    responseStream.Close();
                }
                else
                {
                    result["message"] = "响应失败";
                }
                return result;
            }
            catch (Exception ex)
            {
                result["message"] = "POST请求异常!异常信息:" + ex.Message;
                return result;
            }

        }
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public static JObject GetRequest(string url)
        {
            JObject result = new JObject();
            result["success"] = false;

            try
            {
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "GET";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
                    {
                        var htmlStr = reader.ReadToEnd();
                        if (!string.IsNullOrEmpty(htmlStr))
                        {
                            result["success"] = true;
                            result["data"] = JObject.Parse(htmlStr);
                        }
                    }
                    responseStream.Close();
                }
                else
                {
                    result["message"] = "响应失败";
                }


                return result;
            }
            catch (Exception ex)
            {
                result["message"] = "GET请求异常!异常信息:" + ex.Message;
                return result;
            }
        }

        /// <summary>
        /// 获取客户端IP地址 其中 HttpContext.Current对象需启动注册
        /// </summary>
        /// <returns>若失败则返回回送地址</returns>
        public static string GetClientAddress()
        {
            ////访问端（有可能是用户，有可能是代理的）IP//最后一个代理服务器 IP 
            //string userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //if (string.IsNullOrEmpty(userHostAddress))
            //{
            //    //代理服务器 IP
            //    if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            //        //您的真实 IP ，经过多个代理服务器时，这个值类似如下：203.98.182.163, 203.98.182.163, 203.129.72.215。
            //        userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
            //}
            //if (string.IsNullOrEmpty(userHostAddress))
            //{
            //    userHostAddress = HttpContext.Current.Request.UserHostAddress;
            //}

            ////最后判断获取是否成功，并检查IP地址的格式
            //if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            //{
            //    return userHostAddress;
            //}
            return "127.0.0.1";
        }

        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }



    }
}
