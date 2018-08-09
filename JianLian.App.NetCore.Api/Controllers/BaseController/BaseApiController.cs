using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JianLian.App.NetCore.Api.Controllers
{
    /// <summary>
    /// 基类api控制器
    /// </summary>
    public class BaseApiController : Controller
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public IActionResult SuccessWithData(object data, string message = "")
        {
            return Json(new { Success = true, Data = data, Message = message });
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public IActionResult Success(string message)
        {
            return Json(new { Success = true, Message = message });
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public IActionResult Failed(string message)
        {
            return Json(new { Success = false, Message = message });
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public IActionResult Error(Exception ex, string message)
        {
            //  Logger.Error(ex, message);
            return Json(new { Success = false, Message = message });
        }

        /// <summary>
        /// 未找到
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public IActionResult NotFound(string message)
        {
            return Json(new { Success = false, Message = message });
        }
    }
}