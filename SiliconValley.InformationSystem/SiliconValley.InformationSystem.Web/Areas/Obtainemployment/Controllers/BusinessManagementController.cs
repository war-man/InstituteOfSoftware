using Newtonsoft.Json.Linq;
using SiliconValley.InformationSystem.Business.Base_SysManage;
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
   
    public class BusinessManagementController : Controller
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
        /// 用户业务类
        /// </summary>
        private Base_UserBusiness base_UserBusiness;
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
        /// 判断在企业专业表中，专业id是否对应这个一个企业Id
        /// </summary>
        /// <param name="EntId">企业ID</param>
        /// <param name="SpeID">专业ID</param>
        /// <returns></returns>
        public bool Existence(string EntId, int SpeeID)
        {

            EntSpeeBus = new EntSpeeBusiness();
            int newEntId = int.Parse(EntId);
            var EntSpeeList = EntSpeeBus.GetIQueryable().Where(a => a.IsDel == false).ToList();
            var entspee = EntSpeeList.Where(a => a.EntID == newEntId && a.SpeID == SpeeID).FirstOrDefault();
            if (entspee != null)
                return true;
            else
                return false;

        }
        /// <summary>
        /// 根据专业ID以及企业ID查找企业专业对像
        /// </summary>
        /// <param name="SpecID">专业id</param>
        /// <param name="EntID">企业id</param>
        /// <returns></returns>
        public EntSpee Existence(string SpecID, string EntID)
        {
            EntSpeeBus = new EntSpeeBusiness();
            int newSpecID = int.Parse(SpecID);
            int newEntId = int.Parse(EntID);
            var EntSpeeList = EntSpeeBus.GetIQueryable().Where(a => a.IsDel == false).ToList();
            return EntSpeeList.Where(a => a.EntID == newEntId && a.SpeID == newSpecID).FirstOrDefault();

        }
        /// <summary>
        /// 根据专业id查找专业对象
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Specialty GetSpecialtyById(int Id)
        {
            SpecBus = new SpecialtyBusiness();
            return SpecBus.GetIQueryable().Where(a => a.Id == Id && a.IsDelete == false).FirstOrDefault();
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
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="limit">一页多少条</param>
        /// <param name="whyshow">查询条件</param>
        /// <param name="EntName">企业名称</param>
        /// <param name="EntContacts">联系人</param>
        /// <returns></returns>
        public ActionResult SearchEnterpriseInfo(int page, int limit, string whyshow, string EntName, string EntContacts)
        {
            Enter = new EnterpriseInfoBusiness();
            coo = new CooperaEnterprisesBusiness();
            var coolist = coo.GetIQueryable().Where(a => a.IsCooper == true).ToList();
            //企业信息
            var EnterList = Enter.GetIQueryable().OrderByDescending(a => a.ID).Where(a => a.IsDel == false).ToList();

            switch (whyshow)
            {
                case "ShowCoop":

                    if (coolist.Count!=0)
                    {
                        foreach (var coo in coolist)
                        {
                            foreach (var enter in EnterList)
                            {
                                if (coo.EnterID != enter.ID)
                                {
                                    EnterList.Remove(enter);
                                }
                            }
                        }
                    }
                    else
                    {
                        EnterList =new List<EnterpriseInfo>();
                    }
                    break;
                case "ShowNoCoop":
                    foreach (var coo in coolist)
                    {
                        foreach (var enter in EnterList)
                        {
                            if (coo.EnterID == enter.ID)
                            {
                                EnterList.Remove(enter);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            var newdata = EnterList.Select(a => new ShowCoopEnterView
            {
                EnterpriseInfoID = a.ID,
                EntName = a.EntName,
                EntAddress = a.EntAddress,
                EntNature = a.EntNature,
                EntWelfare = a.EntWelfare,
                EntScale = a.EntScale,
                EmploymentStaffName = GetEmployeesInfoByID(GetEmploymentStaffByID(a.EmpStaffID).EmployeesInfo_Id).EmpName,
                Remark = a.Remark,

                CooperaEnterprisesID = coo.GetCooByEnterID(a.ID)==null?-1:coo.GetCooByEnterID(a.ID).ID,
                EntContacts = coo.GetCooByEnterID(a.ID) == null ? "" : coo.GetCooByEnterID(a.ID).EntContacts,
                EntPhone = coo.GetCooByEnterID(a.ID) == null ? "" : coo.GetCooByEnterID(a.ID).EntPhone,
                EntDate = a.EntDate
            }).ToList();
            if (EntName != null)
            {
                newdata = newdata.Where(a => a.EntName.Contains(EntName)).ToList();
            }
            if (EntContacts != null)
            {
                newdata = newdata.Where(a => a.EntContacts.Contains(EntContacts)).ToList();
            }
            var bnewdata = newdata.Skip((page - 1) * limit).Take(limit).ToList();
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = newdata.Count(),
                data = bnewdata
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        /// <summary>
        /// 根据id接触合作关系
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DelEnterpriseInfo(int CooID)
        {
            //没有进行验证处理。
            coo = new CooperaEnterprisesBusiness();
            AjaxResult ajaxResult = new AjaxResult();
            var cooOld = coo.GetIQueryable().Where(a => a.ID == CooID).FirstOrDefault();
            cooOld.IsCooper = false;
            try
            {
                coo.Update(cooOld);
                ajaxResult.Success = true;
                ajaxResult.Msg = "取消合作";
            }
            catch (Exception ex)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = ex.Message;
            }
            
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddEnterprise()
        {
            SpecBus = new SpecialtyBusiness();
            var Sepclist = SpecBus.GetIQueryable().Where(a => a.IsDelete == false).ToList();
            ViewBag.Sepclist = Sepclist;
            return View();
        }

        /// <summary>
        /// post添加合作企业
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <param name="EntSpeeList"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddEnterprise(AddEnterpriseView jsonStr, string EntSpeeList)
        {
            //现在只采用周炯这个员工  员工编号201908150003  专员id 2 区域id 1
            Enter = new EnterpriseInfoBusiness();
            coo = new CooperaEnterprisesBusiness();
            CooperaEnterprises cooperaEnterprises = new CooperaEnterprises();
            EnterpriseInfo enterpriseInfo = new EnterpriseInfo();
            var AjaxResultss = new AjaxResult();

            //添加企业基础信息
            enterpriseInfo.EmpStaffID = 2;
            enterpriseInfo.EntAddress = jsonStr.EntAddress;
            enterpriseInfo.EntDate = DateTime.Now;
            enterpriseInfo.EntName = jsonStr.EntName;
            enterpriseInfo.EntNature = jsonStr.EntNature;
            enterpriseInfo.EntScale = jsonStr.EntScale;
            enterpriseInfo.EntWelfare = jsonStr.EntWelfare;
            enterpriseInfo.IsDel = false;
            enterpriseInfo.Remark = jsonStr.Remark;
           
            try
            {
                Enter.Insert(enterpriseInfo);
                AjaxResultss.Success = true;
                AjaxResultss.Msg = "添加企业基础信息成功";
                Enter = new EnterpriseInfoBusiness();

                var newEnt = Enter.GetIQueryable().Where(a => a.EntName == jsonStr.EntName).FirstOrDefault();
                var EnterID = newEnt.ID;
                cooperaEnterprises.Date = DateTime.Now;
                cooperaEnterprises.EntContacts = jsonStr.EntContacts;
                cooperaEnterprises.EntPhone = jsonStr.EntPhone;
                cooperaEnterprises.IsCooper = true;
                cooperaEnterprises.Remark = string.Empty;
                cooperaEnterprises.EnterID = EnterID;
                try
                {
                    coo.Insert(cooperaEnterprises);
                    AjaxResultss.Success = true;
                    AjaxResultss.Msg = "添加合作企业成功";
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
                        entSpee.EntSalary = jdata["EntSalary"].ToString();
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

        /// <summary>
        /// 编辑合作企业页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditEnterprise(string id)
        {
            //var UserID = SessionHelper.Session["UserId"].ToString();
            //var user= base_UserBusiness.GetIQueryable().Where(a => a.UserId == UserID).FirstOrDefault();
            //var EmpStaffNo = user.EmpNumber;

            Enter = new EnterpriseInfoBusiness();

            //EmployStaff = new EmploymentStaffBusiness();
            //var MrDEmployStaff = EmployStaff.GetIQueryable().Where(a => a.EmployeesInfo_Id == EmpStaffNo).FirstOrDefault();

            int ID = int.Parse(id);
            var entdata = Enter.GetIQueryable().Where(a => a.ID == ID).FirstOrDefault();
            //if (entdata.EmpStaffID== MrDEmployStaff.ID)
            //{
            coo = new CooperaEnterprisesBusiness();
            EntSpeeBus = new EntSpeeBusiness();
            SpecBus = new SpecialtyBusiness();

            var CooByEnterID = coo.GetCooByEnterID(entdata.ID);
            var modeldata = new EditEnterpriseView();
            modeldata.EntID = entdata.ID;
            modeldata.EntName = entdata.EntName;
            modeldata.EntAddress = entdata.EntAddress;
            modeldata.EntScale = entdata.EntScale;
            modeldata.EntNature = entdata.EntNature;
            modeldata.EntWelfare = entdata.EntWelfare;
            modeldata.Remark = entdata.Remark;
            modeldata.OperNO = CooByEnterID == null ? 0 : 1;
            modeldata.EntContacts = CooByEnterID == null ? "" : CooByEnterID.EntContacts;
            modeldata.EntPhone = CooByEnterID == null ? "" : CooByEnterID.EntPhone;
            modeldata.CooID = CooByEnterID == null ? -1 : CooByEnterID.ID;

            ViewBag.EntSpeelist = EntSpeeBus.GetIQueryable().Where(a => a.EntID == entdata.ID).ToList();

            var Sepclist = SpecBus.GetIQueryable().Where(a => a.IsDelete == false).ToList();
            var SepclistView = Sepclist.Select(a => new MrDSpecialtyView
            {
                Id = a.Id,
                SpecialtyName = a.SpecialtyName,
                IsCheack = Existence(id, a.Id)
            }).ToList();
            ViewBag.SepclistView = SepclistView;
            return View(modeldata);
            //}
            //else
            //{
            //   var  AjaxResult = new AjaxResult();
            //    AjaxResult.Success = false;
            //    AjaxResult.Msg = "你没有权限修改其他人的合作企业信息";
            //    return Json(AjaxResult, JsonRequestBehavior.AllowGet);
            //}


        }

        [HttpPost]
        public ActionResult EditEnterprise(EditEnterpriseView editEnterpriseView, string EntSpeeList)
        {
            int CooID = editEnterpriseView.CooID;
            int OperNo = editEnterpriseView.OperNO;
            var AjaxResultss = new AjaxResult();
            switch (CooID)
            {
                //没有合作企业信息的
                case -1:
                    switch (OperNo)
                    {
                        //也不添加合作企业跟往常一样，只是修改了公司基础信息，以及公司跟专业表
                        case 0:
                            var aa=  Operation( editEnterpriseView,  EntSpeeList);
                            AjaxResultss.Msg = aa.Msg;
                            AjaxResultss.Success = aa.Success;
                            break;
                        //添加合作企业，也修改了公司基础信息，以及公司跟专业表
                        case 1:
                            var newcoo = new CooperaEnterprises();
                            newcoo.EntContacts = editEnterpriseView.EntContacts;
                            newcoo.EntPhone = editEnterpriseView.EntPhone;
                            newcoo.EnterID = editEnterpriseView.EntID;
                            newcoo.IsCooper = true;
                            newcoo.Date = DateTime.Now;
                            newcoo.Remark = string.Empty;
                            try
                            {
                                coo = new CooperaEnterprisesBusiness();
                                coo.Insert(newcoo);
                                var bb = Operation(editEnterpriseView, EntSpeeList);
                                AjaxResultss.Msg = bb.Msg;
                                AjaxResultss.Success = bb.Success;
                            }
                            catch (Exception ex)
                            {
                                AjaxResultss.Success = false;
                                AjaxResultss.Msg = ex.Message;
                            }
                            break;
                    }
                    break;
                //有合作企业信息的
                default:
                    switch (OperNo)
                    {
                        //删除合作企业信息。
                        case 0:
                            coo = new CooperaEnterprisesBusiness();
                            var CooOld=coo.GetCooByID(CooID);
                            CooOld.IsCooper = false;
                            try
                            {
                                coo.Update(CooOld);
                                var cc = Operation(editEnterpriseView, EntSpeeList);
                                AjaxResultss.Msg = cc.Msg;
                                AjaxResultss.Success = cc.Success;
                            }
                            catch (Exception ex)
                            {

                                AjaxResultss.Success = false;
                                AjaxResultss.Msg = ex.Message;
                            }
                            break;
                        //修改合作企业信息。
                        case 1:
                            coo = new CooperaEnterprisesBusiness();
                            var caseonCooOld = coo.GetCooByID(CooID);
                            caseonCooOld.EntContacts = editEnterpriseView.EntContacts;
                            caseonCooOld.EntPhone  = editEnterpriseView.EntPhone;
                            try
                            {
                                coo.Update(caseonCooOld);
                                var dd = Operation(editEnterpriseView, EntSpeeList);
                                AjaxResultss.Msg = dd.Msg;
                                AjaxResultss.Success = dd.Success;
                            }
                            catch (Exception ex)
                            {

                                AjaxResultss.Success = false;
                                AjaxResultss.Msg = ex.Message;
                            }
                            break;
                    }
                    break;
            }
            return Json(AjaxResultss,JsonRequestBehavior.AllowGet);
        }

        private AjaxResult Operation(EditEnterpriseView editEnterpriseView, string EntSpeeList) {
            AjaxResult AjaxResultss = new AjaxResult();
            EntSpeeBus = new EntSpeeBusiness();
            Enter = new EnterpriseInfoBusiness();
            int EetID = editEnterpriseView.EntID;
            var EnterOld = Enter.GetIQueryable().Where(a => a.ID == EetID).FirstOrDefault();
            EnterOld.EntAddress = editEnterpriseView.EntAddress;
            EnterOld.EntName = editEnterpriseView.EntName;
            EnterOld.EntNature = editEnterpriseView.EntNature;
            EnterOld.EntScale = editEnterpriseView.EntScale;
            EnterOld.EntWelfare = editEnterpriseView.EntWelfare;
            EnterOld.Remark = editEnterpriseView.Remark;
            try
            {
                Enter.Update(EnterOld);
                AjaxResultss.Success = true;
                AjaxResultss.Msg = "修改企业成功";
                JArray jArray = JArray.Parse(EntSpeeList);

                List<EntSpee> entspeeList = EntSpeeBus.GetIQueryable().Where(a => a.EntID == EetID).ToList();
                try
                {
                    EntSpeeBus.Delete(entspeeList);
                    AjaxResultss.Success = true;
                    AjaxResultss.Msg = "删除企业专业成功";
                    List<EntSpee> entSpees = new List<EntSpee>();
                    foreach (var item in jArray)
                    {
                        JObject jdata = (JObject)item;
                        var entSpee = new EntSpee();
                        entSpee.EntID = EetID;
                        entSpee.IsDel = false;
                        entSpee.JobResponsibilities = jdata["JobResponsibilities"].ToString();
                        entSpee.Remark = string.Empty;
                        entSpee.Requirements = jdata["Requirements"].ToString();
                        entSpee.SpeeDate = DateTime.Now;
                        entSpee.SpeID = (int)jdata["SpeID"];
                        entSpee.EntSalary = jdata["EntSalary"].ToString();
                        entSpees.Add(entSpee);
                    }
                    try
                    {
                        EntSpeeBus.Insert(entSpees);
                        AjaxResultss.Success = true;
                        AjaxResultss.Msg = "替换企业专业成功";
                    }
                    catch (Exception ex)
                    {

                        AjaxResultss.Success = false;
                        AjaxResultss.Msg = "替换企业专业失败";
                    }
                }
                catch (Exception ex)
                {

                    AjaxResultss.Success = false;
                    AjaxResultss.Msg = "删除企业专业失败";
                }

                
            }
            catch (Exception ex)
            {

                AjaxResultss.Success = false;
                AjaxResultss.Msg = "修改企业失败";
            }
            return AjaxResultss;
        }

        /// <summary>
        /// 用来显示企业专业的信息
        /// </summary>
        /// <param name="SpecID"></param>
        /// <param name="EntID"></param>
        /// <returns></returns>
        public ActionResult GetEntSpeeInfo(string SpecID, string EntID)
        {
            var EntSpee = Existence(SpecID, EntID);
            SpecBus = new SpecialtyBusiness();
            int newSpecID = int.Parse(SpecID);
            var MrDSpec = SpecBus.GetIQueryable().Where(a => a.Id == newSpecID).FirstOrDefault();
            var ResultObj = new
            {
                JobResponsibilities = EntSpee.JobResponsibilities,
                Requirements = EntSpee.Requirements,
                EntSalary = EntSpee.EntSalary,
                SpecialtyName = MrDSpec.SpecialtyName
            };
            AjaxResult ajaxResult = new AjaxResult();
            ajaxResult.Success = true;
            ajaxResult.Msg = "1";
            ajaxResult.Data = ResultObj;
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断公司名称是否唯一
        /// </summary>
        /// <param name="EntName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EntNameOnly(string EntName, int? EntID)
        {
            coo = new CooperaEnterprisesBusiness();
            Enter = new EnterpriseInfoBusiness();
            var AjaxResultss = new AjaxResult();
            var Enterdata = Enter.GetIQueryable().Where(a => a.ID == EntID).FirstOrDefault();
            if (Enterdata.EntName == EntName)
            {
                AjaxResultss.Success = true;
                AjaxResultss.ErrorCode = 1;
                AjaxResultss.Msg = "没问题。";
            }
            else
            {
                try
                {
                    var enterobjj = Enter.GetIQueryable().Where(a => a.EntName == EntName).FirstOrDefault();
                    if (enterobjj != null)
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
            }

            return Json(AjaxResultss, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 判断合作企业的联系人电话是否唯一
        /// </summary>
        /// <param name="EntPhoneOn"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EntPhoneOnly(string EntPhone, int? EntID)
        {
            coo = new CooperaEnterprisesBusiness();
           
            var AjaxResultss = new AjaxResult();
 
            var MrDCoo= coo.GetCooByEnterID(EntID);
            if (MrDCoo != null && MrDCoo.EntPhone == EntPhone)
            {
                AjaxResultss.Success = true;
                AjaxResultss.ErrorCode = 1;
                AjaxResultss.Msg = "没问题。";
            }
            else
            {
                try
                {
                    var enterobjj = coo.GetIQueryable().Where(a => a.EntPhone == EntPhone&&a.IsCooper==true).FirstOrDefault();
                    if (enterobjj != null)
                    {
                        AjaxResultss.Success = true;
                        AjaxResultss.ErrorCode = 0;
                        AjaxResultss.Msg = "存在重复的电话号码。";
                    }
                }
                catch (Exception ex)
                {
                    AjaxResultss.Success = false;
                    AjaxResultss.ErrorCode = 1;
                    AjaxResultss.Msg = "请刷新页面。";
                }
            }

            return Json(AjaxResultss, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Enterdetail(int id) {

            Enter = new EnterpriseInfoBusiness();
            coo = new CooperaEnterprisesBusiness();
            
            EntSpeeBus = new EntSpeeBusiness();
            SpecBus = new SpecialtyBusiness();
            var enterobj = Enter.GetEnterByID(id);
            var MrDCoo = coo.GetCooByEnterID(enterobj.ID);
            EnterdetailView enterdetailView = new EnterdetailView();
            enterdetailView.EntAddress = enterobj.EntAddress;
            enterdetailView.EntName = enterobj.EntName;
            enterdetailView.EntNature = enterobj.EntNature;
            enterdetailView.EntScale = enterobj.EntScale;
            enterdetailView.EntWelfare = enterobj.EntWelfare;
            enterdetailView.Remark = enterobj.Remark;
            enterdetailView.EntContacts = MrDCoo == null ? "" : MrDCoo.EntContacts;
            enterdetailView.EntPhone = MrDCoo == null ? "" : MrDCoo.EntPhone;

            var ListEntSpee = EntSpeeBus.GetIQueryable().Where(a => a.EntID == id).ToList().Select(a => new MrDDetailSpecView
            {
                JobResponsibilities = a.JobResponsibilities,
                Requirements = a.Requirements,
                EntSalary = a.EntSalary,
                SpecialtyName = GetSpecialtyById(a.SpeID).SpecialtyName
            }).ToList();

            ViewBag.NowSpee = Newtonsoft.Json.JsonConvert.SerializeObject(ListEntSpee);

            return View(enterdetailView);
        }
        #endregion
    }
}