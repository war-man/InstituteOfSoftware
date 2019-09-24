using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Dormitory.Controllers
{
    /// <summary>
    /// 宿舍管理控制器
    /// </summary>
    public class DormitoryInfoController : Controller
    {

        // GET: Dormitory/DormitoryInfo

        /// <summary>
        /// 主页面。
        /// </summary>
        /// <returns></returns>
        public ActionResult DormitoryIndex()
        {
            return View();
        }
    }
}