using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace Gym.App.Common
{

    public static class UrlShortHelper
    {
        private static readonly string ShortUrlService = ConfigurationManager.AppSettings["ShortUrlService"];

        public static string GetShortUrl(string url)
        {
            string postUrl = ShortUrlService + "Short";

            string PostObjectJson = JsonConvert.SerializeObject(url);


            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;
            httpRequest = (HttpWebRequest)WebRequest.Create(postUrl);

            httpRequest.Method = "Post";
            httpRequest.ContentType = "application/json; charset=utf-8";
            httpRequest.Accept = "application/json";

            try
            {
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    string json = PostObjectJson;

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                Logger.Error(ex, "网络异常");
                //httpResponse = (HttpWebResponse)ex.Response;
                return JsonConvert.DeserializeObject<String>(string.Empty);
            }

            string str = "";
            try
            {
                Stream st = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
                str = reader.ReadToEnd();
            }
            catch (NullReferenceException ex)
            {
                Logger.Error(ex, "找不到API服务器链接" + url);
                return JsonConvert.DeserializeObject<String>(string.Empty);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "找不到API服务器链接" + url);
                return JsonConvert.DeserializeObject<String>(string.Empty);
            }

            //如果Http的返回值不是OK,则直接返回
            if (httpResponse.StatusCode != HttpStatusCode.OK)
            {
                Logger.Warn("API服务器返回了异常:" + httpResponse.StatusCode + ".");
                return JsonConvert.DeserializeObject<String>(string.Empty);
            }


            String result = JsonConvert.DeserializeObject<String>(str);

            return ShortUrlService + result;
        }

    }
}
