using Newtonsoft.Json.Linq;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    public class EmploymentController : Controller
    {
        /// <summary>
        /// 合作企业业务类
        /// </summary>
        private CooperaEnterprisesBusiness coo;
        /// <summary>
        /// 企业业务类
        /// </summary>
        private EnterpriseInfoBusiness Enter;
        /// <summary>
        /// 员工业务类
        /// </summary>
        private EmployeesInfoManage Employ;
        /// <summary>
        /// 就业专员业务类
        /// </summary>
        private EmploymentStaffBusiness EmployStaff;
        /// <summary>
        /// 专业业务类
        /// </summary>
        private SpecialtyBusiness SpecBus;
        /// <summary>
        /// 企业专业业务类
        /// </summary>
        private EntSpeeBusiness EntSpeeBus;
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
            SpecBus = new SpecialtyBusiness();
            var Sepclist = SpecBus.GetIQueryable().Where(a => a.IsDelete == false).ToList();
            ViewBag.Sepclist = Sepclist;
            return View();
        }

        [HttpPost]
        public ActionResult AddEnterprise(AddEnterpriseView jsonStr, string EntSpeeList)
        {
            //现在只采用周炯这个员工  员工编号190809006  专员id 1

            Enter = new EnterpriseInfoBusiness();
            coo = new CooperaEnterprisesBusiness();

            CooperaEnterprises cooperaEnterprises = new CooperaEnterprises();
            EnterpriseInfo enterpriseInfo = new EnterpriseInfo();
            cooperaEnterprises.Date = DateTime.Now;
            cooperaEnterprises.EntContacts = jsonStr.EntContacts;
            cooperaEnterprises.EntPhone = jsonStr.EntPhone;
            cooperaEnterprises.IsCooper = false;
            cooperaEnterprises.Remark = string.Empty;
            var  AjaxResultss = new AjaxResult();
            try
            {
                coo.Insert(cooperaEnterprises);
                coo = new CooperaEnterprisesBusiness();
                var newCoop = coo.GetIQueryable().Where(a => a.EntPhone == jsonStr.EntPhone).FirstOrDefault();
                var CoopID = newCoop.ID;
                enterpriseInfo.CooID = CoopID;
                enterpriseInfo.EmpStaffID = 1;
                enterpriseInfo.EntAddress = jsonStr.EntAddress;
                enterpriseInfo.EntDate = DateTime.Now;
                enterpriseInfo.EntName = jsonStr.EntName;
                enterpriseInfo.EntNature = jsonStr.EntNature;
                enterpriseInfo.EntSalary = jsonStr.EntSalary;
                enterpriseInfo.EntScale = jsonStr.EntScale;
                enterpriseInfo.EntWelfare = jsonStr.EntWelfare;
                enterpriseInfo.IsDel = false;
                enterpriseInfo.Remark = enterpriseInfo.Remark;
                try
                {
                    Enter.Insert(enterpriseInfo);
                    Enter = new EnterpriseInfoBusiness();
                    var newEnterpriseInfo = Enter.GetIQueryable().Where(a => a.CooID == CoopID).FirstOrDefault();
                    var EnterID = newEnterpriseInfo.ID;
                    JArray jArray = JArray.Parse(EntSpeeList);
                    foreach (var item in jArray)
                    {
                        JObject jdata = (JObject)item;
                        EntSpee entSpee = new EntSpee();
                        entSpee.EntID = EnterID;
                        entSpee.IsDel = false;
                        entSpee.JobResponsibilities = jdata["JobResponsibilities"].ToString();
                        entSpee.Remark = string.Empty;
                        entSpee.Requirements = jdata["Requirements"].ToString();
                        entSpee.SpeeDate = DateTime.Now;
                        entSpee.SpeID = (int)jdata["SpeID"];
                        EntSpeeBus = new EntSpeeBusiness();
                        try
                        {
                            EntSpeeBus.Insert(entSpee);
                            AjaxResultss.Success = true;
                            AjaxResultss.Msg = "添加成功";


                        }
                        catch (Exception ex)
                        {

                            AjaxResultss.Success = false;
                            AjaxResultss.Msg = ex.Message;

                        }
                    }
                }
                catch (Exception ex)
                {

                    AjaxResultss.Success = false;
                    AjaxResultss.Msg = ex.Message;

                }
            }
            catch (Exception ex)
            {
                AjaxResultss.Success = false;
                AjaxResultss.Msg = ex.Message;

            }
            return Json(AjaxResultss, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult EntNameOnly(string EntName)
        {
            var AjaxResultss = new AjaxResult();
            Enter = new EnterpriseInfoBusiness();
            try
            {
                var enterobj = Enter.GetIQueryable().Where(a => a.EntName == EntName).FirstOrDefault();
                if (enterobj!=null)
                {
                    AjaxResultss.Success = true;
                    AjaxResultss.ErrorCode = 0;
                    AjaxResultss.Msg = "该公司信息已存在。";
                }
            }
            catch (Exception ex)
            {
                AjaxResultss.Success = false;
                AjaxResultss.ErrorCode = 1;
                AjaxResultss.Msg = "请刷新页面。";
            }
            return Json(AjaxResultss,JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}