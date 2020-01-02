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
            MeritsCheckManage msrmanage = new MeritsCheckManage();//员工月度工资
            var time = msrmanage.GetList().Where(s => s.IsDel == false).FirstOrDefault().YearAndMonth;
            string mytime = DateTime.Parse(time.ToString()).Year + "年" + DateTime.Parse(time.ToString()).Month + "月";
            ViewBag.yearandmonth = mytime;
            return View();
        }

        /// <summary>
        /// 获取绩效考核的数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult PerformanceAssessShow(int page,int limit,string AppCondition) {
            MeritsCheckManage mcmanage = new MeritsCheckManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var mclist = mcmanage.GetList().Where(s=>s.IsDel==false).ToList();
            if (!string.IsNullOrEmpty(AppCondition))
            {
                string[] str = AppCondition.Split(',');
                string ename = str[0];
                string deptname = str[1];
                string pname = str[2];
                string Empstate = str[3];
                mclist = mclist.Where(e => emanage.GetInfoByEmpID(e.EmployeeId).EmpName.Contains(ename)).ToList();
                if (!string.IsNullOrEmpty(deptname))
                {
                    mclist = mclist.Where(e => emanage.GetDeptByEmpid(e.EmployeeId).DeptId == int.Parse(deptname)).ToList();
                }
                if (!string.IsNullOrEmpty(pname))
                {
                    mclist = mclist.Where(e => emanage.GetPositionByEmpid(e.EmployeeId).Pid == int.Parse(pname)).ToList();
                }
                if (!string.IsNullOrEmpty(Empstate))
                {
                    mclist = mclist.Where(e => emanage.GetInfoByEmpID(e.EmployeeId).IsDel == bool.Parse(Empstate)).ToList();
                }

            }
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

        /// <summary>
        /// 年月份及应到勤天数的改变
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangeMCTime()
        {
            MeritsCheckManage msrmanage = new MeritsCheckManage();
            var time = msrmanage.GetList().Where(s => s.IsDel == false).FirstOrDefault().YearAndMonth;
            string mytime = DateTime.Parse(time.ToString()).Year + "-" + DateTime.Parse(time.ToString()).Month;
            ViewBag.time = mytime;
            return View();
        }
        [HttpPost]
        public ActionResult ChangeMCTime(string CurrentTime)
        {
            var AjaxResultxx = new AjaxResult();
            MeritsCheckManage msrmanage = new MeritsCheckManage();
            try
            {
                var attlist = msrmanage.GetList().Where(s => s.IsDel == false).ToList();
                for (int i = 0; i < attlist.Count(); i++)
                {
                    attlist[i].YearAndMonth = Convert.ToDateTime(CurrentTime);
                    msrmanage.Update(attlist[i]);
                    AjaxResultxx = msrmanage.Success();
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = msrmanage.Error(ex.Message);
            }

            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
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