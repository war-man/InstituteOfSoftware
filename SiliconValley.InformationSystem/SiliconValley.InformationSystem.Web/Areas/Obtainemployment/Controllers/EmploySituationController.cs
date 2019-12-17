using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    public class EmploySituationController : Controller
    {
        private EmploymentJurisdictionBusiness dbemploymentJurisdiction;
        private EmploymentStaffBusiness dbemploymentStaff;
        private QuarterBusiness dbquarter;
        private EmploySituationBusiness dbemploySituation;
        private EmpStaffAndStuBusiness dbempStaffAndStu;
        private SpecialtyBusiness dbspecialty;
        private EnterpriseInfoBusiness dbenterpriseInfo;
        private EntSpeeBusiness dbentSpee;

        // GET: Obtainemployment/EmploySituation
        public ActionResult EmploySituationIndex()
        {
            return View();
        }
        /// <summary>
        /// 加载左侧的树形
        /// </summary>
        /// <returns></returns>
        public ActionResult EstablishTree()
        {
            dbemploySituation = new EmploySituationBusiness();
            var result=dbemploySituation.loadtree();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="leave">1为年度，2为计划</param>
        /// <param name="string1">学生编号</param>
        /// <param name="int">eg: 2019 或者是计划id 7</param>
        /// <returns></returns>
        public ActionResult table00(int page, int limit,  string string1)
        {
            dbemploySituation = new EmploySituationBusiness();
            dbemploymentJurisdiction = new EmploymentJurisdictionBusiness();
            dbemploymentStaff = new EmploymentStaffBusiness();
            dbempStaffAndStu = new EmpStaffAndStuBusiness();
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var queryempstaff = dbemploymentStaff.GetEmploymentByEmpInfoID(user.EmpNumber);
            bool isJurisdiction = dbemploymentJurisdiction.isstaffJurisdiction(user);

            List<EmpStaffAndStuView> data = dbempStaffAndStu.Conversioned(isJurisdiction, queryempstaff.ID);
            if (!string.IsNullOrEmpty(string1))
            {
                List<string> selfobtainid = string1.Split('-').ToList();
                for (int i = data.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < selfobtainid.Count; j++)

                    {
                        if (data[i].StudentNO.ToString() != selfobtainid[j])
                        {
                            if (j == selfobtainid.Count - 1)
                            {
                                data.Remove(data[i]);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            var data1 = data.OrderByDescending(a => a.Salary).Skip((page - 1) * limit).Take(limit).ToList();
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = data.Count,
                data = data1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        /// <summary>
        /// 就业登记
        /// </summary>
        /// <param name="param0">学生编号</param>
        /// <returns></returns>
        public ActionResult employed(string param0) {
            dbempStaffAndStu = new EmpStaffAndStuBusiness();
            dbspecialty = new SpecialtyBusiness();
            ViewBag.Sepclist= Newtonsoft.Json.JsonConvert.SerializeObject(dbspecialty.GetSpecialties());
            ViewBag.obj = Newtonsoft.Json.JsonConvert.SerializeObject(dbempStaffAndStu.studentnoconversionempstaffandstubiew(param0));
            return View();
        }
        [HttpPost]
        /// <summary>
        /// 就业登记
        /// </summary>
        /// <returns></returns>
        public ActionResult employed()
        {
            return View();
        }

        /// <summary>
        /// 搜索公司
        /// </summary>
        /// <param name="param0">公司名称</param>
        /// <returns></returns>
        public ActionResult getentlist(string param0) {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbentSpee = new EntSpeeBusiness();
                dbenterpriseInfo = new EnterpriseInfoBusiness();
                ajaxResult.Data = dbenterpriseInfo.GetAll().Where(a => a.EntName.Contains(param0)).ToList().Select(a => new
                {
                    a.EntName,
                    a.EntAddress,
                    a.EntNature,
                    a.EntScale,
                    a.EntWelfare,
                    a.ID,
                    entspee = dbentSpee.Getentstrintg(a.ID)
                }).ToList();
                ajaxResult.Success = true;
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);

        }

        public ActionResult entlist() {
            return View();
        }
        [HttpGet]
        /// <summary>
        /// 就业登记
        /// </summary>
        /// <returns></returns>
        public ActionResult unemployed(int param0)
        {
            return View();
        }
        [HttpPost]
        /// <summary>
        /// 就业登记
        /// </summary>
        /// <returns></returns>
        public ActionResult unemployed()
        {
            return View();
        }
    }
}