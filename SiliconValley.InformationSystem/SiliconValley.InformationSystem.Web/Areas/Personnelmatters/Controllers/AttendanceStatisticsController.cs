using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    public class AttendanceStatisticsController : Controller
    {
        //考勤统计
        // GET: Personnelmatters/AttendanceStatistics
        public ActionResult AttendanceStatisticsIndex()
        {
            return View();
        }
        //获取考勤数据
        public ActionResult GetCheckingInData(int page, int limit)
        {
            return View();
        }
    }
}