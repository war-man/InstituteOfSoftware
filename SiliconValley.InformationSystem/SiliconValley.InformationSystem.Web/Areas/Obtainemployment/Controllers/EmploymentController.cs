using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    public class EmploymentController : Controller
    {
        //合作企业业务类
        private CooperaEnterprisesBusiness coo;
        //企业业务类
        private EnterpriseInfoBusiness Enter;
        //员工业务类
        private EmployeesInfoManage Employ;
        //就业专员业务类
        private EmploymentStaffBusiness EmployStaff;
        /// <summary>
        /// 根据编号获取员工信息
        /// </summary>
        /// <param name="EmployeesInfo_Id"></param>
        /// <returns></returns>
        public EmployeesInfo GetEmployeesInfoByID(string EmployeesInfo_Id)
        {
            Employ = new EmployeesInfoManage();
            return Employ.GetIQueryable().Where(a => a.EmployeeId == EmployeesInfo_Id).FirstOrDefault();
        }
        /// <summary>
        /// 根据就业专员ID获取就业专员信息
        /// </summary>
        /// <param name="EmpStaffID"></param>
        /// <returns></returns>
        public EmploymentStaff GetEmploymentStaffByID(int? EmpStaffID)
        {
            EmployStaff = new EmploymentStaffBusiness();
            return EmployStaff.GetIQueryable().Where(a => a.ID == EmpStaffID).FirstOrDefault();

        }
        /// <summary>
        /// 根据id获取合作企业对象
        /// </summary>
        /// <param name="CooID"></param>
        /// <returns></returns>
        public CooperaEnterprises GetCooperaEnterprisesByID(int? CooID)
        {
            coo = new CooperaEnterprisesBusiness();
            return coo.GetIQueryable().Where(a => a.ID == CooID).FirstOrDefault();
        }
        #region 关于企业信息库管理
        /// <summary>
        /// 显示合作企业信息
        /// </summary>
        /// <returns></returns>
        public ActionResult EnterpriseInfoIndex()
        {
            return View();
        }


        public ActionResult SearchEnterpriseInfo()
        {
            coo = new CooperaEnterprisesBusiness();
            Enter = new EnterpriseInfoBusiness();
            var EnterList = Enter.GetIQueryable().Where(a => a.IsDel == false).ToList();
            var newdata = EnterList.Select(a => new ShowCoopEnterView
            {
                EnterpriseInfoID = a.ID,
                EntName = a.EntName,
                EntAddress = a.EntAddress,
                EntScale = a.EntSalary,
                EntNature = a.EntNature,
                EntSalary = a.EntSalary,
                EntWelfare = a.EntWelfare,
                EmploymentStaffName = GetEmployeesInfoByID(GetEmploymentStaffByID(a.EmpStaffID).EmployeesInfo_Id).EmpName,
                Remark = a.Remark,
                CooperaEnterprisesID = a.CooID == null ? 0 : a.CooID,
                EntContacts = a.CooID == null ? "暂不是合作企业" : GetCooperaEnterprisesByID(a.CooID).EntContacts,
                EntPhone = a.CooID == null ? "暂不是合作企业" : GetCooperaEnterprisesByID(a.CooID).EntPhone,
            });
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = newdata.Count(),
                data = newdata
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DelEnterpriseInfo(string id)
        {
            return null;
        }
        [HttpGet]
        public ActionResult EditEnterprise(string id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditEnterprise(string ahah, string bb)
        {
            return View();
        }
        [HttpGet]
        public ActionResult AddEnterprise()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AddEnterprise(string id)
        {
            return null;
        }
        #endregion
    }
}