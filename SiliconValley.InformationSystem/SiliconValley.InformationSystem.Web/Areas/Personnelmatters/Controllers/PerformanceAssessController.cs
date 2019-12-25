using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;

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
            var mclist = mcmanage.GetList().Where(s=>s.IsDel==false).ToList();
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
                             routineWorkPropotion=e.RoutineWorkPropotion<=1 && e.RoutineWorkPropotion>0? (e.RoutineWorkPropotion*100)+"%": e.RoutineWorkPropotion>1?e.RoutineWorkPropotion+"%":null,
                             routineWorkFillRate=e.RoutineWorkFillRate<=1&& e.RoutineWorkFillRate>0?(e.RoutineWorkFillRate*100)+"%":e.RoutineWorkFillRate>1? e.RoutineWorkFillRate + "%":null,
                             e.OtherWork,
                             otherWorkPropotion=e.OtherWorkPropotion<=1&&e.OtherWorkPropotion>0?(e.OtherWorkPropotion*100)+"%":e.OtherWorkPropotion>1? e.OtherWorkPropotion + "%":null,
                             otherWorkFillRate=e.OtherWorkFillRate<=1&&e.OtherWorkFillRate>0?(e.OtherWorkFillRate*100)+"%":e.OtherWorkFillRate>1? e.OtherWorkFillRate + "%":null,
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

        public ActionResult EditEmpPFAssess(int id) {
            MeritsCheckManage mcmanage = new MeritsCheckManage();
             var mc=mcmanage.GetEntity(id);
            ViewBag.id = id;
            return View(mc);
        }
        public ActionResult GetMCByid(int id) {
            MeritsCheckManage mcmanage = new MeritsCheckManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var mc = mcmanage.GetEntity(id);
            var mcobj = new {
                mc.Id,
                mc.EmployeeId,
                empName=empmanage.GetInfoByEmpID(mc.EmployeeId).EmpName,
                empmanage.GetInfoByEmpID(mc.EmployeeId).Sex,
                mc.YearAndMonth,
                mc.RoutineWork,
                mc.RoutineWorkPropotion,
                mc.RoutineWorkFillRate,
                mc.OtherWork,
                mc.OtherWorkPropotion,
                mc.OtherWorkFillRate,
                mc.SelfReportedScore,
                mc.SuperiorGrade,
                mc.FinalGrade,
                mc.Remark,
                mc.IsDel
            };
            return Json(mcobj,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditEmpPFAssess(MeritsCheck mc) {
            var AjaxResultxx = new AjaxResult();
            MeritsCheckManage mcmanage = new MeritsCheckManage();
            try
            {
                var m = mcmanage.GetEntity(mc.Id);
                mc.EmployeeId = m.EmployeeId;
                mc.YearAndMonth = m.YearAndMonth;
                mc.IsDel = m.IsDel;
                mcmanage.Update(mc);
                AjaxResultxx = mcmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = mcmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }
    }
}