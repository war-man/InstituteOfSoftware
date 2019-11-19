using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;

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
            AttendanceInfoManage attmanage = new AttendanceInfoManage();
            var attlist=attmanage.GetList().Where(s=>s.IsDel==false).ToList();
            var newlist = attlist.OrderBy(s => s.AAttendanceId).Skip((page - 1) * limit).Take(limit).ToList();
            var mylist = from e in newlist
                         select new
                         {
                             #region 获取值
                             e.AAttendanceId,
                             e.EmployeeId,
                             e.YearAndMonth,
                             e.ToRegularDays,
                             e.LeaveDays,
                             e.WorkAbsentNum,
                             e.WorkAbsentRecord,
                             e.TardyNum,
                             e.TardyRecord,
                             e.LeaveEarlyNum,
                             e.LeaveEarlyRecord,
                             e.Remark,
                             e.IsDel
                             #endregion

                         };

            var newobj = new
            {
                code = 0,
                msg = "",
                count = attlist.Count(),
                data = mylist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }
    }
}