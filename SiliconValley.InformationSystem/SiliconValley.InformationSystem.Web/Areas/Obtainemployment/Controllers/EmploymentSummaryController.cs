using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    [CheckLogin]
    /// <summary>
    /// 总结
    /// </summary>
    public class EmploymentSummaryController : Controller
    {
        private QuarterBusiness dbquarter;
        private EmpStaffAndStuBusiness dbempStaffAndStu;
        private EmploymentStaffBusiness dbEmploymentStaff;
        private EmployeesInfoManage dbemployeesInfoManage;
        // GET: Obtainemployment/EmploymentSummary
        public ActionResult EmploymentSummaryIndex()
        {

            return View();
        }

        /// <summary>
        /// 加载树
        /// </summary>
        /// <returns></returns>
        public ActionResult EstablishTree()
        {

            dbquarter = new QuarterBusiness();
            var result = dbquarter.loadtree();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 加载员工
        /// </summary>
        /// <param name="param0">等级，1为年，2为季度</param>
        /// <param name="param1">值 2019 或者 1007</param>
        /// <returns></returns>
        public ActionResult loadempstaff(bool param0,int param1) {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbEmploymentStaff = new EmploymentStaffBusiness();
                dbemployeesInfoManage = new EmployeesInfoManage();
                var data = dbEmploymentStaff.GetSummaryStaffs(param0, param1);
                ajaxResult.Data = data.Select(a => new {
                    a.ID,
                    staffname = dbemployeesInfoManage.GetEntity(a.EmployeesInfo_Id).EmpName
                }).ToList();
                ajaxResult.Success = true;
            }
            catch (Exception ex)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
    }
}