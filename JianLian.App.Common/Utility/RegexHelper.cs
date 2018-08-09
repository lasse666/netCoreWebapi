using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gym.App.Common
{
    public class RegexHelper
    {
        /// <summary>
        /// 邮箱
        /// </summary>
        public const string Email = @"^[\w-]+@[\w-]+\.[\w-]+$";
        /// <summary>
        /// 中国手机号
        /// </summary>
        public const string ChinaMobile = "^1[3456789][0-9]{9}$";
        /// <summary>
        /// 整数
        /// </summary>
        public const string Digits = @"^-?[\d]+$";
        /// <summary>
        /// 整数和小数
        /// </summary>
        public const string Decimal = @"^-?[\d]+(\.[\d]+)?$";
        /// <summary>
        /// 电话号码
        /// </summary>
        public const string Tel = @"^\+?[\d\s-]+$";

        public const string Password = "^([a-zA-Z0-9]|[!#\\$%&'\\*\\+\\-\\/=\\?\\^_`{\\|}~]){6,20}$";


        private static Regex RegCHZN = new Regex("[一-龥]");
        private static Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
        private static Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$");
        private static Regex RegEmail = new Regex(@"^[\w-]+@[\w-]+\.(com|net|org|edu|mil|tv|biz|info)$");
        private static Regex RegMobilePhone = new Regex("^13|14|15|17|18[0-9]{9}$");
        private static Regex RegMoney = new Regex("^[0-9]+|[0-9]+[.]?[0-9]+$");
        private static Regex RegNumber = new Regex("^[0-9]+$");
        private static Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
        private static Regex RegSend = new Regex("[0-9]{1}([0-9]+){5}");
        private static Regex RegTell = new Regex("^(([0-9]{3,4}-)|[0-9]{3.4}-)?[0-9]{7,8}$");
        private static Regex RegUrl = new Regex(@"^[a-zA-z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?|[a-zA-z]+://((?:(?:25[0-5]|2[0-4]\d|[01]?\d?\d)\.){3}(?:25[0-5]|2[0-4]\d|[01]?\d?\d))$");
        private static Regex RegColor = new Regex(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
    }
}
