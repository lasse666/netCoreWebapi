using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gym.App.Common
{
    public class AnalyzeNoticeParameters
    {
        /// <summary>
        /// 消息模板解析
        /// </summary>
        /// <param name="msgContent">消息内容模板</param>
        /// <param name="dicParas">参数内容字典</param>
        /// <returns></returns>
        public static string AnalyzeParameters(string msgContent, Dictionary<string, string> dicParas)
        {

            if (!string.IsNullOrEmpty(msgContent) && dicParas != null)
            {
                foreach (var item in dicParas)
                {
                    if (msgContent.Contains(item.Key))
                    {
                            msgContent = msgContent.Replace(item.Key, item.Value);
                    }
                }
                return msgContent;
            }

            return string.Empty;
        }
        public static Dictionary<string, string> GetParametersDic()
        {
            //{ Time}
            //时间 { User}
            //职员 / 会员  { Number}
            //数量 { Course}
            //课程 { CourseType}
            //课程类型  { Product}
            //产品 { Price}
            //价格 { UserType}
            //教练或会籍或会员或职员 { Area}
            //小区 { Room}
            //房号 { Bed}
            //床位
            //{ Address}
            //地址
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("", string.Empty);

            return dic;
        }
    }
}
