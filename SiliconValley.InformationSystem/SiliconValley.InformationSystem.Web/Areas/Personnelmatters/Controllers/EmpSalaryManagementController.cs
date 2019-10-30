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

        //绩效考核统计显示
        public ActionResult PerformanceAssessShow() {
            return View();
        }
        /// <summary>
        /// 获取绩效考核的数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetPerformanceAssessData(int page,int limit) {
            MeritsCheckManage mcmanage = new MeritsCheckManage();
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var mclist = mcmanage.GetList();
            var newlist = mclist.Skip((page - 1) * limit).Take(limit).ToList();
            var etlist = from e in newlist
                         select new
                         {
                             #region 获取属性值 
                             e.Id,
                             e.EmployeeId,
                             empName = emanage.GetInfoByEmpID(e.EmployeeId).EmpName,
                             empDept = emanage.GetDept(emanage.GetInfoByEmpID(e.EmployeeId).PositionId).DeptName,
                             empPosition=emanage.GetPositionByEmpid(e.EmployeeId).PositionName,
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

            return Json(newobj,JsonRequestBehavior.AllowGet);
        }
    }
}