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
    public class EmplSalaryEmbodyController : Controller//员工工资体系
    {
        
        // GET: Personnelmatters/EmplSalaryEmbody
        public ActionResult EmpSalaryEmbodyIndex()
        {
            return View();
        }

        //获取员工工资体系表所有数据
        public ActionResult GetSalarySystemData(int page, int limit)
        {
            EmplSalaryEmbodyManage empsemanage = new EmplSalaryEmbodyManage();//员工工资体系表
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var eselist = empsemanage.GetList();
            var newlist = eselist.OrderBy(e => e.Id).Skip((page - 1) * limit).Take(limit);
            var mylist = from e in newlist
                         select new
                         {
                             #region 获取值
                             e.Id,
                             e.EmployeeId,
                             empName = empmanage.GetEntity(e.EmployeeId).EmpName,//姓名
                             Depart = empmanage.GetDeptByEmpid(e.EmployeeId).DeptName,//部门
                             Position = empmanage.GetPositionByEmpid(e.EmployeeId).PositionName,//岗位
                             e.BaseSalary,
                             e.PositionSalary,
                             e.PerformancePay,
                             e.PersonalSocialSecurity,
                             e.SocialSecuritySubsidy,
                             e.NetbookSubsidy,
                             e.ContributionBase,
                             e.PersonalIncomeTax,
                             e.Remark
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

        /// <summary>
        /// 编辑员工工资体系表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditESE(int id) {
            EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
            var ese = esemanage.GetEntity(id);
            ViewBag.eseid = id;
            return View(ese);
        }
        [HttpPost]
        public ActionResult EditESE(EmplSalaryEmbody ese) {
            var AjaxResultxx = new AjaxResult();
            EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
            try
            {
                var myese = esemanage.GetEntity(ese.Id);
                ese.IsDel = myese.IsDel;
                esemanage.Update(ese);
                AjaxResultxx= esemanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = esemanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetESEById(int id)
        {
            EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var ese = esemanage.GetEntity(id);
            var newobj = new {
                ese.Id,
                ese.EmployeeId,
                empName=empmanage.GetEntity(ese.EmployeeId).EmpName,
                deptName=empmanage.GetDeptByEmpid(ese.EmployeeId).DeptName,
                pName=empmanage.GetPositionByEmpid(ese.EmployeeId).PositionName,
                ese.BaseSalary,
                ese.PositionSalary,
                ese.PerformancePay,
                ese.NetbookSubsidy,
                ese.PersonalSocialSecurity,
                ese.SocialSecuritySubsidy,
                ese.ContributionBase,
                ese.PersonalIncomeTax,
                ese.Remark,
                ese.IsDel
            };
            return Json(newobj,JsonRequestBehavior.AllowGet);
       }

        

    }
}