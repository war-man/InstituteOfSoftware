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
    /// <summary>
    /// 学生谈话
    /// </summary>
    public class SimulatInterviewController : Controller
    {
        private EmpClassBusiness dbempClass;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private ProStudentInformationBusiness dbproStudentInformation;
        private ProClassSchedule dbproClassSchedule;
        private SimulatInterviewBusiness dbsimulatInterview;
        private EmploymentStaffBusiness dbemploymentStaff;
        // GET: Obtainemployment/SimulatInterview
        public ActionResult SimulatInterviewIndex()
        {
            //1：获取登陆用户的信息 但是我是在用测试阶段 所以使用一个简单的 empid  杨雪：201908220012
            dbempClass = new EmpClassBusiness();
            var list = dbempClass.GetEmpClassesByempinfoid("201908220012");
            var aa = list.Select(a => new
            {
                ClassNumber = a.ClassId
            }).ToList();
            ViewBag.list = Newtonsoft.Json.JsonConvert.SerializeObject(aa);
            return View();
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