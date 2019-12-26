using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    //[CheckLogin]
    /// <summary>
    /// 学生第一次访谈
    /// </summary>
    public class SResearchRecordController : Controller
    {
        private EmpClassBusiness dbempClass;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private ProStudentInformationBusiness dbproStudentInformation;
        private SpecialtyBusiness dbspecialty;
        private ProClassSchedule dbproClassSchedule;
        private SurveyRecordsBusiness dbsurveyRecords;
        private EmploymentStaffBusiness dbemploymentStaff;
        private EmploymentJurisdictionBusiness dbemploymentJurisdiction;
        private CDInterviewBusiness dbcDInterview;
        // GET: Obtainemployment/SResearchRecord
        public ActionResult SResearchRecordIndex()
        {
            return View();

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
        public ActionResult SResearchRecordRegister(SurveyRecords param0)
        {
            dbemploymentStaff = new EmploymentStaffBusiness();
            Base_UserModel user = Base_UserBusiness.GetCurrentUser();
            var query = dbemploymentStaff.GetEmploymentStaffByempid(user.EmpNumber);
            AjaxResult ajaxResult = new AjaxResult();
            dbsurveyRecords = new SurveyRecordsBusiness();
            try
            {
                param0.EmpStaffID = query.ID;
                param0.IsDel = false;
                param0.RecordsDate = DateTime.Now;
                dbsurveyRecords.Insert(param0);
                ajaxResult.Success = true;
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
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
                dbspecialty = new SpecialtyBusiness();
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
        /// 左侧表格
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="param0"></param>
        /// <param name="param1"></param>
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
            var aa = dbempClass.Conversion(result);

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
        /// 右侧
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult SearchData(int page, int limit, string param0, string param1)
        {
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbsurveyRecords = new SurveyRecordsBusiness();
            dbproStudentInformation = new ProStudentInformationBusiness();
            dbemploymentStaff = new EmploymentStaffBusiness();
            dbspecialty = new SpecialtyBusiness();
            var list2 = new List<SurveyRecords>();
            if (!string.IsNullOrEmpty(param0))
            {
                var list = dbproScheduleForTrainees.GetTraineesByClassid(int.Parse(param0));
              
                var list1 = dbsurveyRecords.GetSurveys();


                foreach (var item in list1)
                {
                    foreach (var item1 in list)
                    {
                        if (item.StudentNO == item1.StudentID)
                        {
                            list2.Add(item);
                        }
                    }
                }
            }
            
            var aa = list2.Select(a => new
            {
                ID = a.ID,
                StudentName = dbproStudentInformation.GetEntity(a.StudentNO).Name,
                EmpStaffName = dbemploymentStaff.GetEmpInfoByEmpID(a.EmpStaffID).EmpName,
                RecordsDate = a.RecordsDate,
                EmpStaffComments = a.EmpStaffComments,
                EmpExpectations = a.EmpExpectations,
                MasterTechnology = a.MasterTechnology,
                PracticalExperience = a.PracticalExperience,
                SelfRating = a.SelfRating,
                SurRating = a.SurRating,
                Remark = a.Remark,
                WantSpceName = dbspecialty.GetEntity(a.WantSpceID).SpecialtyName
            }).ToList();
            if (!string.IsNullOrEmpty(param1))
            {
                aa = aa.Where(a => a.StudentName == param1).ToList();
            }
            var resultdata1 = aa.OrderByDescending(a => a.RecordsDate).Skip((page - 1) * limit).Take(limit).ToList();

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
        /// 修改
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult edit(int param0) {
            dbsurveyRecords = new SurveyRecordsBusiness();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbproStudentInformation = new ProStudentInformationBusiness();
           var data=  dbsurveyRecords.GetEntity(param0);
           var data1= dbproScheduleForTrainees.GetTraineesByStudentNumber(data.StudentNO);
           var data2= dbproStudentInformation.GetEntity(data.StudentNO);
            ViewBag.data = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            ViewBag.data1 = data1.ClassID;
            ViewBag.data2 = data2.Name;
            return View();

        }
        /// <summary>
        /// 修改 如果修改的是a或者是b 那么我就需要查询他第二次cd 访谈是否有记录，如果有记录全部删除掉 
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult edit(SurveyRecords param0)
        {

            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbemploymentStaff = new EmploymentStaffBusiness();
                var query = dbemploymentStaff.Getloginuser();
                dbsurveyRecords = new SurveyRecordsBusiness();
                var data = dbsurveyRecords.GetEntity(param0.ID);
                data.EmpExpectations = param0.EmpExpectations;
                data.EmpStaffComments = param0.EmpStaffComments;
                
                data.EmpStaffID = query.ID;
                data.MasterTechnology = param0.MasterTechnology;
                data.PracticalExperience = param0.PracticalExperience;
                data.RecordsDate = DateTime.Now;
                data.Remark = param0.Remark;
                data.SelfRating = param0.SelfRating;
                data.SurRating = param0.SurRating;
                data.WantSpceID = param0.WantSpceID;
                dbsurveyRecords.Update(data);
                if (data.SurRating.ToUpper()=="A"||data.SurRating.ToUpper()=="B")
                {
                    dbcDInterview = new CDInterviewBusiness();
                    var querylist = dbcDInterview.GetCDsByStudentnumber(data.StudentNO);
                    foreach (var item in querylist)
                    {
                        item.IsDel = true;
                        dbcDInterview.Update(item);
                    }
                }
                ajaxResult.Success = true;
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// 获取专业
        /// </summary>
        /// <returns></returns>
        public ActionResult getWantSpce() {

            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbspecialty = new SpecialtyBusiness();
                var specialtylist = dbspecialty.GetIQueryable().Where(a => a.IsDelete == false).ToList();

                var Specialtylist = specialtylist.Select(a => new
                {
                    Id = a.Id,
                    SpecialtyName = a.SpecialtyName
                }).ToList();
                ajaxResult.Success = true;
                ajaxResult.Data = Specialtylist;
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
          
        }


        /// <summary>
        /// 删除这条访谈 判断这条访谈是否是最近的第一条，如果是：删除掉，判断最近的第一条是否C或者D，如果是：不删除数据，如果不是删除CD访谈所有记录，
        /// 如果不睡最近一条删除该访谈记录即可
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult del(int param0) {

            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbsurveyRecords = new SurveyRecordsBusiness();
                List<SurveyRecords> surveyRecords = new List<SurveyRecords>();
                SurveyRecords survey = dbsurveyRecords.GetEntity(param0);
                survey.IsDel = true;
                dbsurveyRecords.Update(survey);

                if (survey.SurRating.ToUpper() == "C" || survey.SurRating.ToUpper() == "D")
                {
                    dbsurveyRecords = new SurveyRecordsBusiness();
                    if (!dbsurveyRecords.judgeStuisCD(survey.StudentNO))
                    {
                        dbcDInterview = new CDInterviewBusiness();
                       List<CDInterview> cDInterviews= dbcDInterview.GetCDsByStudentnumber(survey.StudentNO);
                        if (cDInterviews!=null)
                        {
                            if (!dbcDInterview.Dellist(cDInterviews))
                            {
                                dbsurveyRecords = new SurveyRecordsBusiness();
                                survey.IsDel = false;
                                dbsurveyRecords.Update(survey);
                                dbcDInterview = new CDInterviewBusiness();
                                dbcDInterview.back(cDInterviews);
                                ajaxResult.Success = false;
                                ajaxResult.Msg = "请联系信息部成员！";
                            }
                        }
                    }
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
    }
}