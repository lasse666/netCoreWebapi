using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gym.App.Common
{
    public class SMSHelper
    {
        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="tel"></param>
        /// <param name="code"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string SendMobileBinding(string tel, string code, string type)
        {
            string msgContent = string.Empty;
            switch (type)
            {
                case "1":
                    msgContent = "您好,您正在使用健身的鱼APP,您获得的验证码是:" + code + "(5分钟内有效)";
                    break;
                case "2":
                    break;
                default:
                    break;
            }

            Task.Run(() =>
            {
                SendSMS(tel, msgContent);
            });

            return code;
        }


        private static bool SendSMS(string tel, string content)
        {
            try
            {
                WebClient client = new WebClient();
                string urlFormat = "http://utf8.sms.webchinese.cn/?Uid={0}&Key={1}&smsMob={2}&smsText={3}";
                string url = string.Format(urlFormat, "jsdeyu", "fba8cf3b2a04a62cb43e", tel, content);
                client.Encoding = Encoding.UTF8;
                string result = client.DownloadString(url);
                return int.Parse(result) > 0;
            }
            catch (Exception)
            {
                return false;
            }


        }
    }
}
