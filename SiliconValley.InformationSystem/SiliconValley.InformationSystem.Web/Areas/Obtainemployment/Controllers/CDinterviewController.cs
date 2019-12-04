using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
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
    public class CDinterviewController : Controller
    {
        private EmpClassBusiness dbempClass;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private ProStudentInformationBusiness dbproStudentInformation;
        private ProClassSchedule dbproClassSchedule;
        private SpecialtyBusiness dbspecialty;
        private CDInterviewBusiness dbcDInterview;
        private SurveyRecordsBusiness dbsurveyRecords;
        private EmploymentStaffBusiness dbemploymentStaff;
        // GET: Obtainemployment/CDinterview
        public ActionResult CDinterviewIndex()
        {
            dbempClass = new EmpClassBusiness();
            var list = dbempClass.GetEmpClassesByempinfoid("201908220012");
            var aa = list.Select(a => new
            {
                ClassNumber =a.ClassId
            }).ToList();
            ViewBag.list = Newtonsoft.Json.JsonConvert.SerializeObject(aa);
            return View();
        }

        [HttpGet]
        /// <summary>
        /// 访谈学生页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SResearchRecordRegister(string param0)
        {
            ViewBag.param0 = param0;
            return View();
        }

        [HttpPost]
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult SResearchRecordRegister(CDInterview param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbcDInterview = new CDInterviewBusiness();
                param0.CDIntDate = DateTime.Now;
                param0.EmpStaffID = 1007;
                param0.IsDel = false;
                dbcDInterview.Insert(param0);
                ajaxResult.Success = true;
            }
            catch (Exception ex)
            {

                ajaxResult.Success = false;
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="param0">班级id</param>
        /// <returns></returns>
        public ActionResult getmudata(int param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbproStudentInformation = new ProStudentInformationBusiness();
                dbproClassSchedule = new ProClassSchedule();
                dbspecialty = new SpecialtyBusiness();
                dbsurveyRecords = new SurveyRecordsBusiness();
                SResearchRecordRegisterView view = new SResearchRecordRegisterView();
                var queryclass = dbproClassSchedule.GetEntity(param0);
                var cdlist = dbsurveyRecords.GetCDSurveyRecordsByclassid(param0);
                List<StudentInformation> studentlist = new List<StudentInformation>();

                foreach (var item in cdlist)
                {
                    studentlist.Add(dbproStudentInformation.GetEntity(item.StudentNO));
                }


                view.Studentlist = studentlist.Select(aa => new
                {
                    StudentNumber = aa.StudentNumber,
                    Name = aa.Name
                }).ToList();

                var specialtylist = dbspecialty.GetIQueryable().Where(a => a.IsDelete == false).ToList();

                view.Specialtylist = specialtylist.Select(a => new
                {
                    Id = a.Id,
                    SpecialtyName = a.SpecialtyName
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="param0">班级编号</param>
        /// <returns></returns>
        public ActionResult SearchData(int page, int limit, string param0,string param1)
        {
           
            dbproStudentInformation = new ProStudentInformationBusiness();
            dbemploymentStaff = new EmploymentStaffBusiness();
            dbspecialty = new SpecialtyBusiness();
            dbcDInterview = new CDInterviewBusiness();
           
            var list1 = dbcDInterview.GetCDInterviewsByClassid(int.Parse(param0));

            var aa = list1.Select(a => new
            {
                ID = a.ID,
                StudentName = dbproStudentInformation.GetEntity(a.StudentNO).Name,
                EmpStaffName = dbemploymentStaff.GetEmpInfoByEmpID(a.EmpStaffID).EmpName,
                CDIntContent=a.CDIntContent,
                Remark = a.Remark,
                CDIntDate=a.CDIntDate,
                WantSpceName = dbspecialty.GetEntity(a.MajorID).SpecialtyName
            }).ToList();
            if (!string.IsNullOrEmpty(param1))
            {
               aa= aa.Where(a => a.StudentName == param1).ToList();
            }
            var resultdata1 = aa.OrderByDescending(a => a.CDIntDate).Skip((page - 1) * limit).Take(limit).ToList();

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