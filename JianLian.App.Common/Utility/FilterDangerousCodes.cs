using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gym.App.Common
{
    /// <summary>
    /// 去除危险字符
    /// </summary>
    public class FilterDangerousCodes
    {
        /// <summary>
        /// 过滤HTML特殊标签
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        public static string FilterHTML(string htmlStr)
        {
            if (htmlStr == null)
                return "";

            string regEx_style = "<style[^>]*?>[\\s\\S]*?<\\/style>"; //定义style的正则表达式 
            string regEx_script = "<script[^>]*?>[\\s\\S]*?<\\/script>"; //定义script的正则表达式 
            string regEx_html = "<[^>]+>"; //定义HTML标签的正则表达式 
            // 定义一些特殊字符的正则表达式 如：&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            string regEx_special = "\\&[a-zA-Z]{1,10};";


            htmlStr = Regex.Replace(htmlStr, regEx_style, "");//删除css
            htmlStr = Regex.Replace(htmlStr, regEx_script, "");//删除js
            htmlStr = Regex.Replace(htmlStr, regEx_html, "");//删除html标记
           // htmlStr = Regex.Replace(htmlStr, "\\s*|\t|\r|\n", "");//去除tab、空格、空行
            htmlStr = Regex.Replace(htmlStr, regEx_special, "");//删除html标记


            htmlStr = htmlStr.Replace(" ", "");
            htmlStr = htmlStr.Replace("\"", "");
            htmlStr = htmlStr.Replace("\n", "");
            

            return htmlStr;
        }
    }
}
