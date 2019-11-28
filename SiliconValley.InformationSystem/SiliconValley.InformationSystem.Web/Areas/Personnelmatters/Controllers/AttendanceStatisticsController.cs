using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    public class AttendanceStatisticsController : Controller
    {
        //考勤统计
        // GET: Personnelmatters/AttendanceStatistics
        public ActionResult AttendanceStatisticsIndex()
        {
            AttendanceInfoManage msrmanage = new AttendanceInfoManage();
            var time = msrmanage.GetList().Where(s => s.IsDel == false).FirstOrDefault().YearAndMonth;
            string mytime = DateTime.Parse(time.ToString()).Year + "年" + DateTime.Parse(time.ToString()).Month + "月";
            ViewBag.yearandmonth = mytime;
            var deserveday = msrmanage.GetList().Where(s => s.IsDel == false).FirstOrDefault().DeserveToRegularDays;
            ViewBag.DeserveToRegularDays = deserveday;
            return View();
        }
        //获取考勤数据
        public ActionResult GetCheckingInData(int page, int limit)
        {
            AttendanceInfoManage attmanage = new AttendanceInfoManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var attlist=attmanage.GetList().Where(s=>s.IsDel==false).ToList();
            var newlist = attlist.OrderBy(s => s.AttendanceId).Skip((page - 1) * limit).Take(limit).ToList();
            var mylist = from e in newlist
                         select new
                         {
                             #region 获取值
                             e.AttendanceId,
                             e.EmployeeId,
                             empName = empmanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             empDept = empmanage.GetDeptByEmpid(e.EmployeeId).DeptName,
                             empPosition=empmanage.GetPositionByEmpid(e.EmployeeId).PositionName,
                             empIsDel=empmanage.GetInfoByEmpID(e.EmployeeId).IsDel,
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
                             e.IsDel,
                             e.DeserveToRegularDays,
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
        /// <summary>
        /// 年月份及应到勤天数的改变
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChangeTimeandDays() {
            AttendanceInfoManage msrmanage = new AttendanceInfoManage();
            var time = msrmanage.GetList().Where(s => s.IsDel == false).FirstOrDefault().YearAndMonth;
            string mytime = DateTime.Parse(time.ToString()).Year + "-" + DateTime.Parse(time.ToString()).Month ;
            ViewBag.time = mytime;
            var deserveday = msrmanage.GetList().Where(s => s.IsDel == false).FirstOrDefault().DeserveToRegularDays;
            ViewBag.days = deserveday;
            return View();
        }
        [HttpPost]
        public ActionResult ChangeTimeandDays(string CurrentTime,int ShouldComeDays) {
            var AjaxResultxx = new AjaxResult();
            AttendanceInfoManage msrmanage = new AttendanceInfoManage();
            try
            {
                var attlist = msrmanage.GetList().Where(s=>s.IsDel==false).ToList();
                for (int i = 0; i < attlist.Count(); i++)
                {
                    attlist[i].YearAndMonth = Convert.ToDateTime(CurrentTime);
                    attlist[i].DeserveToRegularDays = ShouldComeDays;
                    msrmanage.Update(attlist[i]);
                    AjaxResultxx = msrmanage.Success();
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = msrmanage.Error(ex.Message);
            }
          
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditAttendanceInfo(int id) {
            AttendanceInfoManage attmanage = new AttendanceInfoManage();
            var att=attmanage.GetEntity(id);
            return View(att);
        }
        public ActionResult GetAttById(int id) {
            AttendanceInfoManage attmanage = new AttendanceInfoManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var att = attmanage.GetEntity(id);
            var newobj = new {
                #region 考勤表赋值
                att.AttendanceId,
                att.EmployeeId,
                empName = emanage.GetInfoByEmpID(att.EmployeeId).EmpName,
                esex = emanage.GetInfoByEmpID(att.EmployeeId).Sex,
                dname = emanage.GetDeptByEmpid(att.EmployeeId).DeptName,
                pname = emanage.GetPositionByEmpid(att.EmployeeId).PositionName,
                att.ToRegularDays,
                att.LeaveDays,
                att.WorkAbsentNum,
                att.WorkAbsentRecord,
                att.OffDutyAbsentNum,
                att.OffDutyAbsentRecord,
                att.TardyNum,
                att.TardyRecord,
                att.LeaveEarlyNum,
                att.LeaveEarlyRecord,
                att.Remark,
                att.YearAndMonth,
                att.DeserveToRegularDays,
                att.IsDel,
                #endregion
            };
            return Json(newobj,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditAttendanceInfo(AttendanceInfo att) {
            var AjaxResultxx = new AjaxResult();
            AttendanceInfoManage atmanage = new AttendanceInfoManage();
            try
            {
                var a = atmanage.GetEntity(att.AttendanceId);
                att.YearAndMonth = a.YearAndMonth;
                att.DeserveToRegularDays = a.DeserveToRegularDays;
                att.IsDel = a.IsDel;
                att.EmployeeId = a.EmployeeId;
                atmanage.Update(att);
                AjaxResultxx = atmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = atmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }

    }
}