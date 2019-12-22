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
    [CheckLogin]
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
            var studentno = "19111999033000015";
            var obj = dbstudentIntention.GetStudnetIntentionByStudentNO(studentno);
            var stuobj= dbproStudentInformation.GetEntity(studentno); 
            StudentIntentionView view = new StudentIntentionView();
            var undres = dbproScheduleForTrainees.GetTraineesByStudentNumber(studentno);
            view.classnumnber = undres.ClassID;
            var studentobj = dbproStudentInformation.GetEntity(studentno);
            view.StudentNO = stuobj.StudentNumber;
            view.sex = studentobj.Sex == true ? "男" : "女";
            view.StudentName = studentobj.Name;
            view.Telephone = studentobj.Telephone;
            if (obj != null)
            {
                ViewBag.existence = true;
                if (obj.AreaID==null)
                {
                    view.AreaName = "其他城市";
                }
                else
                {
                    var areasobj = dbemploymentAreas.GetEntity(obj.AreaID);
                    view.AreaName = areasobj.AreaName;
                }
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
        public ActionResult typing(StudentIntentionView param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbstudentIntention = new StudentIntentionBusiness();
                var StudnetIntentionobj=  dbstudentIntention.GetStudnetIntentionByStudentNO(param0.StudentNO);
                
                if (StudnetIntentionobj==null)
                {
                    dbproStudentInformation = new ProStudentInformationBusiness();
                    var studentobj= dbproStudentInformation.GetStudent(param0.StudentNO);
                    if (studentobj.Telephone!=param0.Telephone)
                    {
                        studentobj.Telephone = param0.Telephone;
                        dbproStudentInformation.Update(studentobj);
                    }
                    StudnetIntention studnetIntention = new StudnetIntention();
                  
                        studnetIntention.AreaID = param0.AreaID;
                   
                   
                    studnetIntention.Salary = param0.Salary;
                    studnetIntention.StudentNO = param0.StudentNO;
                    studnetIntention.IsDel = false;
                    studnetIntention.Remark = string.Empty;
                    studnetIntention.Date = DateTime.Now;
                    dbstudentIntention.Insert(studnetIntention);
                    ajaxResult.Success = true;
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