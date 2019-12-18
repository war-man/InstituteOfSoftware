using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var result = dbemploySituation.loadtree();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="string1">学生编号</param>
        /// <returns></returns>
        public ActionResult table00(int page, int limit, string string1)
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
                data= data.Where(a => a.StudentName.Contains(string1)).ToList();
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
        public ActionResult employed(string param0)
        {
            dbempStaffAndStu = new EmpStaffAndStuBusiness();
            dbspecialty = new SpecialtyBusiness();
            ViewBag.Sepclist = Newtonsoft.Json.JsonConvert.SerializeObject(dbspecialty.GetSpecialties());
            ViewBag.obj = Newtonsoft.Json.JsonConvert.SerializeObject(dbempStaffAndStu.studentnoconversionempstaffandstubiew(param0));
            return View();
        }

        /// <summary>
        /// 名字是否存在
        /// </summary>
        /// <param name="param0">公司名字</param>
        /// <param name="param1">公司id</param>
        /// <returns></returns>
        public ActionResult isonleyname(string param0, string param1)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {

                dbenterpriseInfo = new EnterpriseInfoBusiness();
                ajaxResult.Data = dbenterpriseInfo.isonlyname(param0, param1);
                ajaxResult.Success = true;
            }
            catch (Exception ex)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        /// <summary>
        /// 就业登记
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult employed(employed_entView param0)
        {



            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                ///首先判断的是这个gongsi id 是否是有值
                ///如果存在值，那就证明这个公司是已经有数据了，不需要进行添加，如果没有值，需要进行添加操作
                dbemploySituation = new EmploySituationBusiness();
                dbempStaffAndStu = new EmpStaffAndStuBusiness();
                dbemploymentStaff = new EmploymentStaffBusiness();
                Base_UserModel user = Base_UserBusiness.GetCurrentUser();
                var queryempstaff = dbemploymentStaff.GetEmploymentByEmpInfoID(user.EmpNumber);
                if (param0.EntinfoID == null)
                {
                    dbenterpriseInfo = new EnterpriseInfoBusiness();
                    dbemploymentStaff = new EmploymentStaffBusiness();
                    dbentSpee = new EntSpeeBusiness();
               
                    ///添加公司
                    EnterpriseInfo enterprise = new EnterpriseInfo();
                    enterprise.EmpStaffID = queryempstaff.ID;
                    enterprise.IsCooper = false;
                    enterprise.IsDel = false;
                    enterprise.Remark = string.Empty;
                    enterprise.EntAddress = param0.EntAddress;
                    enterprise.EntContacts = string.Empty;
                    var now = DateTime.Now;
                    enterprise.EntDate = now;
                    enterprise.EntName = param0.EntName;
                    enterprise.EntNature = param0.EntNature;
                    enterprise.EntPhone = string.Empty;
                    enterprise.EntScale = param0.EntScale;
                    enterprise.EntWelfare = string.Empty;
                    dbenterpriseInfo.Insert(enterprise);


                    ///查询刚刚添加的数据
                    dbenterpriseInfo = new EnterpriseInfoBusiness();
                    EnterpriseInfo queryobj = dbenterpriseInfo.GetNoCooAll().Where(a => a.EntDate.ToString() == now.ToString()).FirstOrDefault();

                    ///修改参数param0 的公司id
                    param0.EntinfoID = queryobj.ID;

                    ///添加该公司的专业
                    List<string> list = param0.EntSpee.Split('-').ToList();
                    foreach (var item in list)
                    {
                        EntSpee entSpee = new EntSpee();
                        entSpee.EntID = queryobj.ID;
                        entSpee.IsDel = false;
                        entSpee.SpeeDate = DateTime.Now;
                        entSpee.SpeID = int.Parse(item);
                        dbentSpee.Insert(entSpee);
                    }
                }
                ///添加就业情况
                EmploySituation employSituation = new EmploySituation();
                employSituation.EntinfoID = param0.EntinfoID;
                employSituation.Date = DateTime.Now;
                employSituation.IsDel = false;
                employSituation.RealWages = param0.RealWages;
                employSituation.Remark = string.Empty;
                employSituation.StudentNO = param0.StudentNO;
                employSituation.empid = queryempstaff.ID;
                dbemploySituation.Insert(employSituation);

                ///修改这个专员带学生记录 改为已就业
                var query = dbempStaffAndStu.GetIsingBystudentno(param0.StudentNO);
                query.EmploymentState = 2;
                dbempStaffAndStu.Update(query);
                ajaxResult.Success = true;
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);



        }
        /// <summary>
        /// 搜索公司
        /// </summary>
        /// <param name="param0">公司名称</param>
        /// <returns></returns>
        public ActionResult getentlist(string param0)
        {
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

        public ActionResult entlist()
        {
            return View();
        }
        [HttpGet]
        /// <summary>
        /// 未就业登记
        /// </summary>
        /// <returns></returns>
        public ActionResult unemployed(string param0)
        {
            dbempStaffAndStu = new EmpStaffAndStuBusiness();
            ViewBag.obj = Newtonsoft.Json.JsonConvert.SerializeObject(dbempStaffAndStu.studentnoconversionempstaffandstubiew(param0));
            return View();
        }
        [HttpPost]
        /// <summary>
        ///未 就业登记
        /// </summary>
        /// <returns></returns>
        public ActionResult unemployed(EmploySituation param0)
        {

            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                ///添加未就业情况
                
                dbemploySituation = new EmploySituationBusiness();
                dbempStaffAndStu = new EmpStaffAndStuBusiness();
                dbemploymentStaff = new EmploymentStaffBusiness();
                param0.Date = DateTime.Now;
                param0.IsDel = false;
                Base_UserModel user = Base_UserBusiness.GetCurrentUser();
                var queryempstaff = dbemploymentStaff.GetEmploymentByEmpInfoID(user.EmpNumber);
                param0.empid = queryempstaff.ID;
                dbemploySituation.Insert(param0);

                ///修改分配记录 这个学生没人带了  
                ///如果这个数据本身就是第二次数据呢？
               var query=  dbempStaffAndStu.GetIsingBystudentno(param0.StudentNO);
                query.Ising = false;
                dbempStaffAndStu.Update(query);

                if (query.EmploymentStage!=1)
                {
                    query.EmpStaffID = null;
                    query.Ising = false;
                }
                else
                {
                    ///添加一个数据 变成第二次就业数据
                    dbempStaffAndStu = new EmpStaffAndStuBusiness();
                    EmpStaffAndStu empStaffAndStu = new EmpStaffAndStu();
                    empStaffAndStu.Date = DateTime.Now;
                    empStaffAndStu.EmploymentStage = 2;
                    empStaffAndStu.EmploymentState = 3;
                    empStaffAndStu.IsDel = false;
                    empStaffAndStu.Ising = false;
                    empStaffAndStu.QuarterID = query.QuarterID;
                    empStaffAndStu.Remark = "登记为未就业学生";
                    empStaffAndStu.Studentno = param0.StudentNO;
                    dbempStaffAndStu.Insert(empStaffAndStu);
                }
                

                ajaxResult.Success = true;
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
         

        }

        /// <summary>
        /// 历史记录
        /// </summary>
        /// <returns></returns>
        public ActionResult EmploySituationSee()
        {
            return View();
        }


        /// <summary>
        /// 就业情况表单
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="string1">学生姓名</param>
        /// <param name="string2">等级 1或者是2  1--年   2--计划</param>
        /// <param name="string3">计划id或者是年份 eg: 1007 季度id & 2019 年度</param>
        /// <returns></returns>
        public ActionResult table01(int page, int limit, string string1,string string2,int int1)
        {
            dbemploySituation = new EmploySituationBusiness();
            dbemploymentJurisdiction = new EmploymentJurisdictionBusiness();
            dbemploymentStaff = new EmploymentStaffBusiness();
            dbempStaffAndStu = new EmpStaffAndStuBusiness();
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var queryempstaff = dbemploymentStaff.GetEmploymentByEmpInfoID(user.EmpNumber);
            bool isJurisdiction = dbemploymentJurisdiction.isstaffJurisdiction(user);
            bool year = false;
            if (string2=="1")
            {
                year = true;
            }
            List<EmploySituationView> data= dbemploySituation.GetSituationsViewByQuarterid(isJurisdiction, year, int1, queryempstaff.ID);

            if (!string.IsNullOrEmpty(string1))
            {
                data = data.Where(a => a.StudentName.Contains(string1)).ToList();
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
    }
}