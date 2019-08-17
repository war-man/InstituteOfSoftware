
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    using SiliconValley.InformationSystem.Business;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Business.Employment;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;

    /// <summary>
    /// 就业专员管理
    /// </summary>
    public class EmploymentCommissionerController : Controller
    {

        /// <summary>
        /// 就业专员业务类
        /// </summary>
        private EmploymentStaffBusiness MrDEmployStaffBus;
        /// <summary>
        /// 就业区域业务类
        /// </summary>
        private EmploymentAreasBusiness MrDEmployAreBus;
        /// <summary>
        /// 基表员工业务类
        /// </summary>
        private EmployeesInfoManage NoMrDEmployManBus;
        /// <summary>
        /// 管理就业专员首页
        /// </summary>
        /// <returns></returns>
        public ActionResult EmploymentCommissionerIndex()
        {
            return View();
        }
        /// <summary>
        /// 显示就业专员的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchData(int page, int limit, string whyshow, string EntName, string EntContacts)
        {
            MrDEmployStaffBus = new EmploymentStaffBusiness();
            MrDEmployAreBus = new EmploymentAreasBusiness();
            var MrDEmpInfoData = MrDEmployStaffBus.GetALl();
            var returnlist = new List<MrDEmployStaffView>();
            foreach (var item in MrDEmpInfoData)
            {
                EmployeesInfo info = MrDEmployStaffBus.GetEmployeesInfoByID(item.EmployeesInfo_Id);
                var obj = new MrDEmployStaffView();
                obj.EmploymentStaffID = item.ID;
                obj.EmpName = info.EmpName;
                obj.EmployeeId = item.EmployeesInfo_Id;
                obj.Sex = info.Sex;
                obj.AreaName = item.AreaID == null ? "" : MrDEmployAreBus.GetObjByID(item.AreaID).AreaName;
                returnlist.Add(obj);
            }
            var bnewdata = returnlist.Skip((page - 1) * limit).Take(limit).ToList();
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = returnlist.Count(),
                data = bnewdata
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 就业专员列表
        /// </summary>
        /// <returns></returns>
        public ActionResult EmpStaffList() {
            return View();
        }

        /// <summary>
        /// 获取可以没有在就业专员表中的而且是在就业部中的员工数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEmpStaffData(int page, int limit) {
            MrDEmployStaffBus = new EmploymentStaffBusiness();
            var returnlist = MrDEmployStaffBus.EmployeesInfos();
            var bnewdata = returnlist.Skip((page - 1) * limit).Take(limit).ToList();
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = returnlist.Count(),
                data = bnewdata
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddEmployStaff(string empid) {
            NoMrDEmployManBus = new EmployeesInfoManage();
            var emp = NoMrDEmployManBus.GetList().Where(d => d.IsDel == false && d.EmployeeId == empid).ToList().FirstOrDefault();

            ViewBag.Emp = emp;

            return View();
        }
    }
}