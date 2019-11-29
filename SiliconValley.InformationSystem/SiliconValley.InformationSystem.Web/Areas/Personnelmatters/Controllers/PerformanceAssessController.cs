using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    public class PerformanceAssessController : Controller
    {
        //绩效考核统计
        // GET: Personnelmatters/PerformanceAssess
        public ActionResult PerformanceAssessIndex()
        {
            return View();
        }

        /// <summary>
        /// 获取绩效考核的数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult PerformanceAssessShow(int page,int limit) {
            MeritsCheckManage mcmanage = new MeritsCheckManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var mclist = mcmanage.GetList().Where(s=>s.IsDel==false);
            var newlist = mclist.Skip((page - 1) * limit).Take(limit).ToList();
            var etlist = from e in newlist
                         select new
                         {
                             #region 获取属性值 
                             e.Id,
                             e.EmployeeId,
                             empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             empDept = emanage.GetDept(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).DeptName,
                             empPosition = emanage.GetPositionByEmpid(e.EmployeeId).PositionName,
                             empIsDel=emanage.GetInfoByEmpID(e.EmployeeId).IsDel,
                             e.YearAndMonth,
                             e.RoutineWork,
                             e.RoutineWorkPropotion,
                             e.RoutineWorkFillRate,
                             e.OtherWork,
                             e.OtherWorkPropotion,
                             e.OtherWorkFillRate,
                             e.SelfReportedScore,
                             e.SuperiorGrade,
                             e.FinalGrade,
                             e.Remark,
                             e.IsDel
                             #endregion
                         };

            var newobj = new
            {
                code = 0,
                msg = "",
                count = mclist.Count(),
                data = etlist
            };

            return Json(newobj, JsonRequestBehavior.AllowGet);
        }
    }
}