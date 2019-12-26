using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    //[CheckLogin]
    /// <summary>
    /// 学生面试记录登记
    /// </summary>
    public class SimulatInterviewController : Controller
    {
        private EmploymentJurisdictionBusiness dbemploymentJurisdiction;
        private EmpClassBusiness dbempClass;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private ProStudentInformationBusiness dbproStudentInformation;
        private ProClassSchedule dbproClassSchedule;
        private SimulatInterviewBusiness dbsimulatInterview;
        private EmploymentStaffBusiness dbemploymentStaff;
        // GET: Obtainemployment/SimulatInterview
        public ActionResult SimulatInterviewIndex()
        {
            return View();
        }
        /// <summary>
        /// 左侧表格
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="param0"></param>
        /// <param name="param1">班级号</param>
        /// <returns></returns>
        public ActionResult SearchData0(int page, int limit, string param0, string param1)
        {

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            dbemploymentJurisdiction = new EmploymentJurisdictionBusiness();
            dbempClass = new EmpClassBusiness();
            dbproClassSchedule = new ProClassSchedule();
            List<EmpClass> result = new List<EmpClass>();
            if (dbemploymentJurisdiction.isstaffJurisdiction(user))
            {
                result = dbempClass.GetIQueryable().Where(a => a.IsDel == false).ToList();
            }
            else
            {
                result = dbempClass.GetEmpClassesByempinfoid(user.EmpNumber);
            }
            switch (param0)
            {
                case "ing":
                    result = dbempClass.Leavebehinding(result);
                    break;
                case "ed":
                    result = dbempClass.Leavebehinded(result);
                    break;

                default:
                    result = dbempClass.Leavebehinding(result);
                    break;
            }

            if (!string.IsNullOrEmpty(param1))
            {
                result = dbempClass.CorrespondingByClassNumber(result, param1);
            }
            var aa = dbempClass.Conversion1(result);

            var resultdata1 = aa.OrderByDescending(a => a.classid).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = aa.Count(),
                data = resultdata1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }     

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult getmudata(int param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbproScheduleForTrainees = new ProScheduleForTrainees();
                dbproStudentInformation = new ProStudentInformationBusiness();
                dbproClassSchedule = new ProClassSchedule();
                SResearchRecordRegisterView view = new SResearchRecordRegisterView();

                var querylist = dbproScheduleForTrainees.GetTraineesByClassid(param0);
                var queryclass = dbproClassSchedule.GetEntity(param0);

                List<StudentInformation> studentlist = new List<StudentInformation>();
                foreach (var item in querylist)
                {
                    studentlist.Add(dbproStudentInformation.GetEntity(item.StudentID));
                }
                view.Studentlist = studentlist.Select(aa => new
                {
                    StudentNumber = aa.StudentNumber,
                    Name = aa.Name
                }).ToList();

  
                ajaxResult.Success = true;
                ajaxResult.Data = view;
            }
            catch (Exception)
            {

                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }

            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        /// <summary>
        /// 访谈学生页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SResearchRecordRegister(int param0)
        {
            dbproClassSchedule = new ProClassSchedule();
            var query = dbproClassSchedule.GetEntity(param0);
            ViewBag.param0 = query.ClassNumber;
            ViewBag.param1 = param0;
            return View();
        }

        [HttpPost]
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult SResearchRecordRegister(SimulatInterview param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
          
            try
            {
                dbsimulatInterview = new SimulatInterviewBusiness();
                param0.AddDate = DateTime.Now;
                param0.EntStaffID = 1007;
                param0.IsDel = false;
                dbsimulatInterview.Insert(param0);
                ajaxResult.Success = true;
            }
            catch (Exception)
            {

                ajaxResult.Success = false;

            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="param0">班级id</param>
        /// <returns></returns>
        public ActionResult SearchData(int page, int limit, string param0)
        {
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbproStudentInformation = new ProStudentInformationBusiness();
            dbemploymentStaff = new EmploymentStaffBusiness();
            dbsimulatInterview = new SimulatInterviewBusiness();
          var dubu=  dbsimulatInterview.GetSimulatInterviewsByclassid(int.Parse(param0));
            var aa = dubu.Select(a => new
            {
                ID = a.ID,
                StudentName = dbproStudentInformation.GetEntity(a.StudentNo).Name,
                EmpStaffName = dbemploymentStaff.GetEmpInfoByEmpID(a.EntStaffID).EmpName,
                AddDate=a.AddDate,
                knowMyself=a.knowMyself,
                a.ObjectInfo,
                a.Pandect,
                a.PerIntQues,
                a.Resume,
                a.ShowAbility,
                Remark = a.Remark
            }).ToList();
            var resultdata1 = aa.OrderByDescending(a => a.AddDate).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = aa.Count(),
                data = resultdata1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }
    }
}