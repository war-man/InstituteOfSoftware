using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    public class EmpSalaryManagementController : Controller
    {
        //员工工资管理页面
        // GET: Personnelmatters/EmpSalaryManagement
        public ActionResult SalaryManageIndex()
        {
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资
            var time = msrmanage.GetList().Where(s=>s.IsDel==false).FirstOrDefault().YearAndMonth;
            string mytime = DateTime.Parse(time.ToString()).Year + "年" + DateTime.Parse(time.ToString()).Month + "月";
            ViewBag.yearandmonth = mytime;
            return View();
        }
        //工资表数据加载
        public ActionResult EmpSalaryData(int page, int limit)
        {
            EmplSalaryEmbodyManage empsemanage = new EmplSalaryEmbodyManage();//员工工资体系表
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资
            EmployeesInfoManage empmanage = new EmployeesInfoManage();//员工信息表
            var eselist = msrmanage.GetList().Where(s => s.IsDel == false).ToList();
            var newlist = eselist.OrderBy(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            
            var mylist = from e in newlist
                         select new
                         {
                             #region 获取值
                             e.Id,
                             e.EmployeeId,
                             empName = empmanage.GetEntity(e.EmployeeId).EmpName,//姓名
                             Depart = empmanage.GetDeptByEmpid(e.EmployeeId).DeptName,//部门
                             Position = empmanage.GetPositionByEmpid(e.EmployeeId).PositionName,//岗位
                             toRegularDays = msrmanage.GetAttendanceInfoByEmpid(e.EmployeeId).ToRegularDays,//到勤天数
                             baseSalary = msrmanage.GetEmpsalaryByEmpid(e.EmployeeId).BaseSalary,//基本工资
                             positionSalary = msrmanage.GetEmpsalaryByEmpid(e.EmployeeId).PositionSalary,//岗位工资
                             finalGrade = msrmanage.GetMCByEmpid(e.EmployeeId).FinalGrade,//绩效分
                             e.PerformanceSalary,//绩效工资
                             netbookSubsidy = msrmanage.GetEmpsalaryByEmpid(e.EmployeeId).NetbookSubsidy,//笔记本补助
                            socialSecuritySubsidy = msrmanage.GetEmpsalaryByEmpid(e.EmployeeId).SocialSecuritySubsidy,//社保补贴
                          //应发工资1(基本工资+岗位工资+绩效工资+笔记本补助+社保补贴)
                             SalaryOne = msrmanage.GetEmpsalaryByEmpid(e.EmployeeId).BaseSalary + msrmanage.GetEmpsalaryByEmpid(e.EmployeeId).PositionSalary + e.PerformanceSalary + msrmanage.GetEmpsalaryByEmpid(e.EmployeeId).NetbookSubsidy + msrmanage.GetEmpsalaryByEmpid(e.EmployeeId).SocialSecuritySubsidy,

                             e.OvertimeCharges,//加班费用
                             e.Bonus, // 奖金/元
                             leavedays = msrmanage.GetAttendanceInfoByEmpid(e.EmployeeId).LeaveDays,//请假天数
                             e.LeaveDeductions,//（请假）扣款/元
                             e.OtherDeductions,//其他扣款
                            
                             //应发工资2(应发工资1+加班费用+奖金-请假扣款-其他扣款)
                             SalaryTwo =(msrmanage.GetEmpsalaryByEmpid(e.EmployeeId).BaseSalary + msrmanage.GetEmpsalaryByEmpid(e.EmployeeId).PositionSalary + e.PerformanceSalary + msrmanage.GetEmpsalaryByEmpid(e.EmployeeId).NetbookSubsidy + msrmanage.GetEmpsalaryByEmpid(e.EmployeeId).SocialSecuritySubsidy) +e.OvertimeCharges+e.Bonus-e.LeaveDeductions-e.OtherDeductions,

                             e.PersonalSocialSecurity,//个人社保
                             msrmanage.GetEmpsalaryByEmpid(e.EmployeeId).PersonalIncomeTax,//个税
                             e.Total,//合计
                             e.PayCardSalary,//工资卡工资
                             e.CasehSalary//现金工资
                             #endregion

                         };

             var newobj = new
            {
                code = 0,
                msg = "",
                count = eselist.Count(),
                data = mylist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

     
    }
}