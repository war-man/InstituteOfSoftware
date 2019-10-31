using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.SchoolAttendanceManagementBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    public class EmpLeaveRequestController : Controller
    {
        // GET: Personnelmatters/EmpLeaveRequest
        public ActionResult LeaveIndex()
        {
            return View();
        }
       
        /// <summary>
        /// 获取所有请假申请数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetEmpLeaveRequestData(int page, int limit)
        {
            LeaveRequestManage lramanage = new LeaveRequestManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var list = lramanage.GetList();
            var newlist = list.OrderByDescending(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var etlist = from e in newlist
                         select new
                         {
                             #region 获取属性值 
                             e.Id,
                             empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             esex = emanage.GetInfoByEmpID(e.EmployeeId).Sex,

                             #endregion
                         };
            var newobj = new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = etlist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }
    }
}