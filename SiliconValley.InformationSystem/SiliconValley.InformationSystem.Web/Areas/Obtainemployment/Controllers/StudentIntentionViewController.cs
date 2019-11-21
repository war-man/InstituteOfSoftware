using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
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
    public class StudentIntentionViewController : Controller
    {

        private ProStudentInformationBusiness dbproStudentInformation;
        private StudentIntentionBusiness dbstudentIntention;
        private EmploymentAreasBusiness dbemploymentAreas;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private StudentDataKeepAndRecordBusiness dbstudentDataKeepAndRecord;
        // GET: Obtainemployment/StudentIntentionView
        public ActionResult StudentIntentionViewIndex()
        {
            //19111999033000015
            //19111990101200014
            dbstudentIntention = new StudentIntentionBusiness();
            dbproStudentInformation = new ProStudentInformationBusiness();
            dbemploymentAreas = new EmploymentAreasBusiness();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbstudentDataKeepAndRecord = new StudentDataKeepAndRecordBusiness();
            var studentno = "19111990101200014";
            var obj = dbstudentIntention.GetStudnetIntentionByStudentNO(studentno);
            var stuobj= dbproStudentInformation.GetEntity(studentno); 
            StudentIntentionView view = new StudentIntentionView();
            var undres = dbproScheduleForTrainees.GetTraineesByStudentNumber(studentno);
            view.classnumnber = undres.ClassID;
            var studentobj = dbproStudentInformation.GetEntity(studentno);
            view.Familyphone = studentobj.Familyphone;
            view.StudentNO = stuobj.StudentNumber;
            var stringlist=  studentobj.Guardian.Split(',');
            view.RelativesName = stringlist[0].Replace(" ","");
            view.Relationship = stringlist[1].Replace(" ", "");
            view.sex = studentobj.Sex == true ? "男" : "女";
            view.StudentName = studentobj.Name;
            view.Telephone = studentobj.Telephone;
            var studentDataKeepAndRecordobj= dbstudentDataKeepAndRecord.GetEntity(studentobj.StudentPutOnRecord_Id);
            view.StuSchoolName = studentDataKeepAndRecordobj.StuSchoolName;
            if (obj != null)
            {
                ViewBag.existence = true;
                var areasobj= dbemploymentAreas.GetEntity(obj.AreaID);
                view.AreaName = areasobj.AreaName;
                view.Salary = obj.Salary;
            }
            else
            {
                ViewBag.existence = false;
                
                var aa = dbemploymentAreas.GetAll();
                var cc = aa.Select(a => new
                {
                    a.AreaName,
                    a.ID
                }).ToList();
                ViewBag.city = Newtonsoft.Json.JsonConvert.SerializeObject(cc);
            }
            ViewBag.myobj = Newtonsoft.Json.JsonConvert.SerializeObject(view);
            return View();
        }

        [HttpPost]
        /// <summary>
        /// 进行添加就业意向表
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult typing(StudnetIntention param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbstudentIntention = new StudentIntentionBusiness();
                var StudnetIntentionobj=  dbstudentIntention.GetStudnetIntentionByStudentNO(param0.StudentNO);
                if (StudnetIntentionobj!=null)
                {
                    param0.IsDel = false;
                    param0.Remark = string.Empty;
                    param0.Date = DateTime.Now;
                }
                else
                {
                    ajaxResult.Success = false;
                    ajaxResult.Msg = "莫乱搞。";
                }
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请及时联系就业班主任!";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
    }
}