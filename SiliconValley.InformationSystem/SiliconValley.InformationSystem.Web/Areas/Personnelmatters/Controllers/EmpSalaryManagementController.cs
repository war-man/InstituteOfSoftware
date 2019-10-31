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
            return View();
        }
        //工资表数据加载
        public ActionResult EmpSalaryData(int page,int limit) {
            EmplSalaryEmbodyManage empsemanage = new EmplSalaryEmbodyManage();//员工工资体系表
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var eselist = empsemanage.GetList().Where(s => s.IsDel == false);
            var newlist = eselist.OrderBy(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var mylist = from e in newlist select new {
                e.Id,
                e.EmployeeId,
                empName = empmanage.GetEntity(e.EmployeeId).EmpName,
                Depart = empmanage.GetDeptByEmpid(e.EmployeeId).DeptName,
                Position=empmanage.GetPositionByEmpid(e.EmployeeId).PositionName,
                e.BaseSalary,
                e.PositionSalary,
                e.PerformancePay,
                e.PersonalSocialSecurity,
                e.SocialSecuritySubsidy,
                e.ContributionBase,
                e.PayCardSalarySum,
                e.PersonalIncomeTax,
                e.Remark,
                e.IsDel

            };

            var newobj = new
            {
                code = 0,
                msg = "",
                count = eselist.Count(),
                data = mylist
            };
            return Json(newobj,JsonRequestBehavior.AllowGet);
        }

      
        
    }
}