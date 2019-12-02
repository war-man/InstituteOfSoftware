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
    public class SResearchRecordController : Controller
    {
        private EmpClassBusiness dbempClass;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private ProStudentInformationBusiness dbproStudentInformation;
        private SpecialtyBusiness dbspecialty;
        private ProClassSchedule dbproClassSchedule;
        private SurveyRecordsBusiness dbsurveyRecords;
        private EmploymentStaffBusiness dbemploymentStaff;
        // GET: Obtainemployment/SResearchRecord
        public ActionResult SResearchRecordIndex()
        {
            //1：获取登陆用户的信息 但是我是在用测试阶段 所以使用一个简单的 empid  杨雪：201908220012
            dbempClass = new EmpClassBusiness();
            var list = dbempClass.GetEmpClassesByempinfoid("201908220012");
            var aa = list.Select(a => new
            {
                ClassNumber = a.ID
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
        public ActionResult SResearchRecordRegister(SurveyRecords param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            dbsurveyRecords = new SurveyRecordsBusiness();
            try
            {
                param0.EmpStaffID = 1007;
                param0.IsDel = false;
                param0.RecordsDate = DateTime.Now;
                dbsurveyRecords.Insert(param0);
                ajaxResult.Success = true;
            }
            catch (Exception)
            {

                ajaxResult.Success = false;

            }
            return Json(ajaxResult,JsonRequestBehavior.AllowGet);
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
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult SearchData(int page, int limit,string param0) {
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbsurveyRecords = new SurveyRecordsBusiness();
            dbproStudentInformation = new ProStudentInformationBusiness();
            dbemploymentStaff = new EmploymentStaffBusiness();
            dbspecialty = new SpecialtyBusiness();
            var list= dbproScheduleForTrainees.GetTraineesByClassid(int.Parse(param0));
            var list1 = dbsurveyRecords.GetSurveys();
            var list2 = new List<SurveyRecords>();

            foreach (var item in list1)
            {
                foreach (var item1 in list)
                {
                    if (item.StudentNO==item1.StudentID)
                    {
                        list2.Add(item);
                    }
                }
            }


          var aa=  list2.Select(a => new
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
             var resultdata1 = aa.OrderByDescending(a => a.SurRating).Skip((page - 1) * limit).Take(limit).ToList();

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