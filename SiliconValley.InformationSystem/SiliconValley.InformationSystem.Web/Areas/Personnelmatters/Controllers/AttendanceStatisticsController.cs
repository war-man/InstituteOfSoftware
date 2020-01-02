using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Util;
using System.Data;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    public class AttendanceStatisticsController : Controller
    {
        //考勤统计
        // GET: Personnelmatters/AttendanceStatistics
        public ActionResult AttendanceStatisticsIndex()
        {
            AttendanceInfoManage msrmanage = new AttendanceInfoManage();
            if (msrmanage.GetList().Where(s => s.IsDel == false).Count() > 0)
            {
                var time = msrmanage.GetList().Where(s => s.IsDel == false).FirstOrDefault().YearAndMonth;
                string mytime = DateTime.Parse(time.ToString()).Year + "年" + DateTime.Parse(time.ToString()).Month + "月";
                ViewBag.yearandmonth = mytime;

                var deserveday = msrmanage.GetList().Where(s => s.IsDel == false).FirstOrDefault().DeserveToRegularDays;
                ViewBag.DeserveToRegularDays = deserveday;
            }

            return View();
        }
        //获取考勤数据
        public ActionResult GetCheckingInData(int page, int limit, string AppCondition)
        {
            AttendanceInfoManage attmanage = new AttendanceInfoManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var attlist = attmanage.GetList().Where(s => s.IsDel == false).ToList();
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
                             e.NoClockWithhold,
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
            AttendanceInfoManage msrmanage = new AttendanceInfoManage();
            if (msrmanage.GetList().Where(s => s.IsDel == false).Count() > 0)
            {
                var time = msrmanage.GetList().Where(s => s.IsDel == false).FirstOrDefault().YearAndMonth;
                string mytime = DateTime.Parse(time.ToString()).Year + "-" + DateTime.Parse(time.ToString()).Month;
                ViewBag.time = mytime;

                var deserveday = msrmanage.GetList().Where(s => s.IsDel == false).FirstOrDefault().DeserveToRegularDays;
                ViewBag.days = deserveday;
            }
            return View();
        }
        [HttpPost]
        public ActionResult ChangeTimeandDays(string CurrentTime, int ShouldComeDays)
        {
            var AjaxResultxx = new AjaxResult();
            AttendanceInfoManage msrmanage = new AttendanceInfoManage();
            try
            {
                var attlist = msrmanage.GetList().Where(s => s.IsDel == false).ToList();
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

            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditAttendanceInfo(int id)
        {
            AttendanceInfoManage attmanage = new AttendanceInfoManage();
            var att = attmanage.GetEntity(id);
            return View(att);
        }
        public ActionResult GetAttById(int id)
        {
            AttendanceInfoManage attmanage = new AttendanceInfoManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var att = attmanage.GetEntity(id);
            var newobj = new
            {
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
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditAttendanceInfo(AttendanceInfo att)
        {
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
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Excel文件导入
        /// </summary>
        /// <returns></returns>
        public ActionResult ToLeadExcelfile()
        {

            DataTable table = AsposeOfficeHelper.ReadExcel(Server.MapPath("/uploadXLSXfile/湖南硅谷云教育科技有限公司_考勤报表_20191101-20191130(1).xlsx"));

            //SqlConnection con = new SqlConnection("server=.;database=exsil;uid=sa;pwd=123;");

            //con.Open();
            //SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con);

            List<string> arry = new List<string> { "1", "2", "3", "4", "5", "六", "日", "姓名", "部门" };

            //sqlBulkCopy.DestinationTableName = "users";
            //sqlBulkCopy.BatchSize = table.Rows.Count;
            foreach (var item in arry)
            {
                try
                {
                    //  sqlBulkCopy.ColumnMappings.Add(item, Guid.NewGuid().ToString());
                }
                catch (Exception)
                {


                }


            }


            //  sqlBulkCopy.WriteToServer(table);

            return Content("ok");
        }
    }
}