
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
    using SiliconValley.InformationSystem.Util;

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
                obj.Phone = info.Phone;
                obj.EmployeeId = item.EmployeesInfo_Id;
                obj.Sex = info.Sex;
                obj.AreaName =MrDEmployAreBus.GetObjByID(item.AreaID).AreaName;
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
        public ActionResult EmpStaffList()
        {
            return View();
        }

        /// <summary>
        /// 获取可以没有在就业专员表中的而且是在就业部中的员工数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEmpStaffData(int page, int limit)
        {

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

        /// <summary>
        /// 显示页面的添加员工
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddEmployStaff(string empid)
        {
           
            NoMrDEmployManBus = new EmployeesInfoManage();
            MrDEmployAreBus = new EmploymentAreasBusiness();
            var AreaData = MrDEmployAreBus.GetAll().Select(s1=>new SelectListItem() { Text=s1.AreaName,Value=s1.ID.ToString()}).ToList();
            SelectListItem s = new SelectListItem() { Text= "---待定---", Value = "-1",Selected=true};
            AreaData.Add(s);
            ViewBag.Area = AreaData;
            var emp = NoMrDEmployManBus.GetList().Where(d => d.IsDel == false && d.EmployeeId == empid).ToList().FirstOrDefault();

            ViewBag.Emp = emp;

            return View();
        }

        /// <summary>
        /// post请求的action
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddEmployStaff(EmploymentStaff employmentStaff)
        {
            MrDEmployStaffBus = new EmploymentStaffBusiness();


            employmentStaff.IsDel = false;
            employmentStaff.Date = DateTime.Now;
            if (employmentStaff.AreaID > 0)
            {
                employmentStaff.AreaID = employmentStaff.AreaID;
            }
            else
            {
                employmentStaff.AreaID = null;
            }
           
            AjaxResult result = new AjaxResult();

            try
            {
                MrDEmployStaffBus.Insert(employmentStaff);
                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = null;
            }
            catch (Exception)
            {

                result.ErrorCode = 500;
                result.Msg = "错误";
                result.Data = null;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 显示详细页面
        /// </summary>
        /// <param name="id">就业专员ID</param>
        /// <returns></returns>
        public ActionResult EmployDetailView(int id)
        {

            MrDEmployStaffBus = new EmploymentStaffBusiness();
            EmployStaffDetailView result = MrDEmployStaffBus.GetStaffDetailView(id);
            return View(result);
        }

        [HttpGet]
        /// <summary>
        /// get 请求的编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditEmployStaff(int id)
        {
            MrDEmployStaffBus = new EmploymentStaffBusiness();
            var empstaff = MrDEmployStaffBus.GetEmploymentByID(id);
            var empinfo = MrDEmployStaffBus.GetEmployeesInfoByID(empstaff.EmployeesInfo_Id);
            ViewBag.empinfo = empinfo;
            MrDEmployAreBus = new EmploymentAreasBusiness();
            var AreaData = MrDEmployAreBus.GetAll().Select(s1 => new SelectListItem() { Text = s1.AreaName, Value = s1.ID.ToString() }).ToList();
            SelectListItem s = new SelectListItem() { Text = "---待定---", Value = "-1", Selected = true };
            AreaData.Add(s);
            ViewBag.Area = AreaData;
            return View(empstaff);
        }

        /// <summary>
        /// get 请求修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditEmployStaff(EmploymentStaff employment)
        {
            AjaxResult result = null;

            MrDEmployStaffBus = new EmploymentStaffBusiness();
            var bubu = MrDEmployStaffBus.GetEmploymentByID(employment.ID);
            bubu.AttendClassStyle = employment.AttendClassStyle.Trim();
            bubu.EmployExperience = employment.EmployExperience.Trim();
            bubu.WorkExperience = employment.WorkExperience.Trim();
            bubu.Remark = employment.Remark.Trim();
            if (employment.AreaID > 0)
            {
                bubu.AreaID = employment.AreaID;
            }
            else
            {
                bubu.AreaID = null;
            }
            try
            {
                MrDEmployStaffBus.Update(bubu);
                result = new SuccessResult();
                result.Msg = "修改成功";
                result.Success = true;

            }
            catch (Exception)
            {
                result = new ErrorResult();
                result.ErrorCode = 500;
                result.Msg = "服务器错误1";

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManageClass(/*string ids*/) {

            //var temparryid = ids.Split(',');
            //var list = temparryid.ToList();
            //list.RemoveAt(temparryid.Length-1);

            return View();
        }
    }
}