using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.ViewEntity.SalaryView;

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
            List<MySalaryObjView> result = new List<MySalaryObjView>();
            
            foreach (var item in newlist)
            {
                MySalaryObjView view = new MySalaryObjView();
                view.Id = item.Id;
                view.EmployeeId = item.EmployeeId;
                view.empName = empmanage.GetEntity(item.EmployeeId).EmpName;
                view.Depart = empmanage.GetDeptByEmpid(item.EmployeeId).DeptName;
                view.Position = empmanage.GetPositionByEmpid(item.EmployeeId).PositionName;
                //考勤表对象
                var attendobj = msrmanage.GetAttendanceInfoByEmpid(item.EmployeeId);
                view.toRegularDays = attendobj.ToRegularDays;
              
                //员工工资体系表
                var eseobj= msrmanage.GetEmpsalaryByEmpid(item.EmployeeId);
                view.baseSalary = eseobj.BaseSalary;
                view.positionSalary = eseobj.PositionSalary;
                view.finalGrade = msrmanage.GetMCByEmpid(item.EmployeeId).FinalGrade;
                view.PerformanceSalary = item.PerformanceSalary;
                view.netbookSubsidy = eseobj.NetbookSubsidy;
                view.socialSecuritySubsidy = eseobj.SocialSecuritySubsidy;
                view.SalaryOne = view.baseSalary + view.positionSalary + view.PerformanceSalary + view.netbookSubsidy + view.socialSecuritySubsidy;

                view.OvertimeCharges = item.OvertimeCharges;
                view.Bonus = item.Bonus;
                view.leavedays = attendobj.LeaveDays;
                view.LeaveDeductions = item.LeaveDeductions;
                view.OtherDeductions = item.OtherDeductions;
                view.SalaryTwo = view.SalaryOne + view.OvertimeCharges + view.Bonus - view.LeaveDeductions + view.OtherDeductions;
                view.PersonalSocialSecurity = item.PersonalSocialSecurity;
                view.PersonalIncomeTax = eseobj.PersonalIncomeTax;
                view.Total = item.Total;
                view.PayCardSalary = item.PayCardSalary;
                view.CasehSalary = item.CasehSalary;
                result.Add(view);
            }
            
             var newobj = new
            {
                code = 0,
                msg = "",
                count = eselist.Count(),
                data = result
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

     
    }
}