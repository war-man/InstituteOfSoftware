﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Util;
using System.Data;
using System.Text;
using System.IO;
using SiliconValley.InformationSystem.Business.Base_SysManage;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    public class AttendanceStatisticsController : Controller
    {
        AttendanceInfoManage atdmanage = new AttendanceInfoManage();
        EmployeesInfoManage empmanage = new EmployeesInfoManage();
        RedisCache rc = new RedisCache();

        //第一次进入月度工资表页面时加载的年月份的方法
        static string GetFirstTime()
        {
            AttendanceInfoManage atdmanage = new AttendanceInfoManage();//员工月度工资
            string mytime = "";
            if (atdmanage.GetADInfoData().Where(s => s.IsDel == false).Count() > 0)
            {
                var time = atdmanage.GetADInfoData().Where(s => s.IsDel == false).LastOrDefault().YearAndMonth;
                mytime = DateTime.Parse(time.ToString()).Year + "-" + DateTime.Parse(time.ToString()).Month;
            }
            else {
                mytime = "";
            }
            return mytime;
        }
        //第一次进入页面加载的应到勤天数
        static string GetFirstDeserveToRegularDays()
        {
            AttendanceInfoManage atdmanage = new AttendanceInfoManage();//员工月度工资
            string myDeserveToRegularDays = "";
            if (atdmanage.GetADInfoData().Where(s => s.IsDel == false).Count() > 0)
            {
                myDeserveToRegularDays = atdmanage.GetADInfoData().Where(s => s.IsDel == false).LastOrDefault().DeserveToRegularDays.ToString();
            }
            return myDeserveToRegularDays;
        }
        static string FirstTime = GetFirstTime();
        static string Firstshouldday = GetFirstDeserveToRegularDays();
        //考勤统计
        // GET: Personnelmatters/AttendanceStatistics
        public ActionResult AttendanceStatisticsIndex()
        {
            ViewBag.yearandmonth = FirstTime;
            ViewBag.DeserveToRegularDays = Firstshouldday;
            return View();
        }
        //获取考勤数据
        public ActionResult GetCheckingInData(int page, int limit, string AppCondition,string ymtime)
        {
            ymtime = FirstTime;
            var attlist = atdmanage.GetADInfoData().Where(s => s.IsDel == false).ToList();
            if (!string.IsNullOrEmpty( ymtime)) {
            var time = DateTime.Parse(ymtime);
             attlist = attlist.Where(s =>DateTime.Parse(s.YearAndMonth.ToString()).Year==time.Year && DateTime.Parse(s.YearAndMonth.ToString()).Month == time.Month).ToList();
            }
            if (!string.IsNullOrEmpty(AppCondition))
            {
                string[] str = AppCondition.Split(',');
                string ename = str[0];
                string deptname = str[1];
                string pname = str[2];
                string empstate = str[3];

                attlist = attlist.Where(e => empmanage.GetInfoByEmpID(e.EmployeeId).EmpName.Contains(ename)).ToList();
                if (!string.IsNullOrEmpty(deptname))
                {
                    attlist = attlist.Where(e => empmanage.GetDeptByEmpid(e.EmployeeId).DeptId == int.Parse(deptname)).ToList();
                }
                if (!string.IsNullOrEmpty(pname))
                {
                    attlist = attlist.Where(e => empmanage.GetInfoByEmpID(e.EmployeeId).PositionId == int.Parse(pname)).ToList();
                }
                if (!string.IsNullOrEmpty(empstate))
                {
                    attlist = attlist.Where(e => empmanage.GetInfoByEmpID(e.EmployeeId).IsDel == bool.Parse(empstate)).ToList();
                }

            }
            var newlist = attlist.OrderBy(s => s.AttendanceId).Skip((page - 1) * limit).Take(limit).ToList();
            var mylist = from e in newlist
                         select new
                         {
                             #region 获取值
                             e.AttendanceId,
                             e.EmployeeId,
                             empName = empmanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             empDept = empmanage.GetDeptByEmpid(e.EmployeeId).DeptName,
                             empPosition = empmanage.GetPositionByEmpid(e.EmployeeId).PositionName,
                             empIsDel = empmanage.GetInfoByEmpID(e.EmployeeId).IsDel,
                             e.YearAndMonth,
                             e.ToRegularDays,
                             e.LeaveDays,
                             e.WorkAbsentNum,
                             e.WorkAbsentRecord,
                             e.OffDutyAbsentNum,
                             e.OffDutyAbsentRecord,
                             NoClockTotalNum = e.WorkAbsentNum + e.OffDutyAbsentNum,
                             e.TardyNum,
                             e.TardyRecord,
                             e.LeaveEarlyNum,
                             e.LeaveEarlyRecord,
                             e.Remark,
                             e.IsDel,
                             e.DeserveToRegularDays,
                             e.TardyWithhold,
                             e.LeaveWithhold
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
        public ActionResult ChangeTimeandDays()
        {
            if (atdmanage.GetADInfoData().Where(s => s.IsDel == false).Count() > 0)
            {
                ViewBag.time = FirstTime; 
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChangeTimeandDays(string CurrentTime)
        {
            var AjaxResultxx = new AjaxResult();
            try
            {
                var stime = DateTime.Parse(CurrentTime);
                var attlist = atdmanage.GetADInfoData().Where(s => s.IsDel == false && DateTime.Parse(s.YearAndMonth.ToString()).Year == stime.Year && DateTime.Parse(s.YearAndMonth.ToString()).Month == stime.Month).ToList();
                if (attlist.Count > 0)
                {
                    FirstTime = CurrentTime;
                    Firstshouldday = attlist[0].DeserveToRegularDays.ToString();
                }
                AjaxResultxx.Success = true;
                AjaxResultxx.Data = attlist.Count();
            }
            catch (Exception ex)
            {
                AjaxResultxx.Success = false;
                AjaxResultxx = atdmanage.Error(ex.Message);
            }


            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditAttendanceInfo(int id)
        {
            var att = atdmanage.GetEntity(id);
            return View(att);
        }
        public ActionResult GetAttById(int id)
        {
            var att = atdmanage.GetEntity(id);
            var newobj = new
            {
                #region 考勤表赋值
                att.AttendanceId,
                att.EmployeeId,
                empName = empmanage.GetInfoByEmpID(att.EmployeeId).EmpName,
                esex = empmanage.GetInfoByEmpID(att.EmployeeId).Sex,
                dname = empmanage.GetDeptByEmpid(att.EmployeeId).DeptName,
                pname = empmanage.GetPositionByEmpid(att.EmployeeId).PositionName,
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
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditAttendanceInfo(AttendanceInfo att)
        {
            var AjaxResultxx = new AjaxResult();
            try
            {
                var a = atdmanage.GetEntity(att.AttendanceId);
                att.YearAndMonth = a.YearAndMonth;
                att.DeserveToRegularDays = a.DeserveToRegularDays;
                att.IsDel = a.IsDel;
                att.EmployeeId = a.EmployeeId;
                atdmanage.Update(att);
                rc.RemoveCache("InRedisATDData");
                AjaxResultxx = atdmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = atdmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BatchImport()
        {
            return View();
        }
        /// <summary>
        /// 批量录入
        /// </summary>
        /// <param name="excelfile"></param>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]

        public ActionResult BatchImport(HttpPostedFileBase excelfile)
        {
            Stream filestream = excelfile.InputStream;

            var result = atdmanage.ImportDataFormExcel(filestream, excelfile.ContentType);

            // Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            if (result.Success) {
                var timeandday = result.Msg;
                string []str = timeandday.Split(',');
                var time = str[0];
                var day = str[1];
                    var mytime = DateTime.Parse(time.ToString()).Year + "-" + DateTime.Parse(time.ToString()).Month;
                FirstTime =mytime;
                Firstshouldday = day;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BatchAdd() {
       
            return View();
        }
        [HttpPost]
        public ActionResult BatchAdd(string list) {
            var AjaxResultxx = new AjaxResult();
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }

    }
}