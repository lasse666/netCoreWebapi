using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Gym.App.Common;
using JianLian.App.IBiz;
using Microsoft.AspNetCore.Mvc;

namespace JianLian.App.NetCore.Api.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Produces("application/json")]
    public class TestController : BaseApiController
    {

        #region 属性声明

        private readonly IStudentService _iStudentService;

        #endregion

        /// <summary>
        /// 构造函数 注入
        /// </summary>
        public TestController(IStudentService iStudentService)
        {
            _iStudentService = iStudentService;
        }

        /// <summary>
        /// 获取学生信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("GetSudentInfo")]
        public IActionResult GetSudentInfo(Guid key)
        {
            //int a = Convert.ToInt32(paramsAB.A);
            //int b = Convert.ToInt32(paramsAB.B);
            var stu = _iStudentService.GetById(key);

            return SuccessWithData(stu, "获取成功");
        }


        /// <summary>
        /// 测试1
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTest1")]
        public IActionResult GetTest1()
        {
            return SuccessWithData(new { Key = Guid.NewGuid() }, "GET 成功");
        }
        

        /// <summary>
        /// 测试2
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetTest2")]
        public string GetTest2()
        {
            string PCManagerSiteTest = ConfigurationManager.AppSettings["PCManagerSite"].ToString();
            return PCManagerSiteTest;
        }


    }
}
