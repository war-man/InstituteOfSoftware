using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    public class StudnetIntentionController : Controller
    {
        private EmpClassBusiness dbempClass;
        //private EIntentionClassXMLHelp eIntentionClassXMLHelp;
        private ProStudentInformationBusiness dbproStudentInformation;
        private StudentIntentionBusiness dbstudentIntention;
        private EmploymentAreasBusiness dbemploymentAreas;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        private StudentDataKeepAndRecordBusiness dbstudentDataKeepAndRecord;
        // GET: Obtainemployment/StudnetIntention
        public ActionResult StudnetIntentionIndex()
        {
            //1：获取登陆用户的信息 但是我是在用测试阶段 所以使用一个简单的 empid  杨雪：201908220012
            dbempClass = new EmpClassBusiness();
            var list = dbempClass.GetEmpClassesByempinfoid("201908220012");
            var aa = list.Select(a => new
            {
                ClassNumber = a.ClassNO
            }).ToList();
            ViewBag.list = Newtonsoft.Json.JsonConvert.SerializeObject(aa);
            return View();
        }

     



        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult SearchData(int page, int limit, string param0)
        {
            dbstudentIntention = new StudentIntentionBusiness();

            dbstudentDataKeepAndRecord = new StudentDataKeepAndRecordBusiness();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbemploymentAreas = new EmploymentAreasBusiness();
            dbproStudentInformation = new ProStudentInformationBusiness();
            var data = dbstudentIntention.GetStudnetIntentionsByclassno(param0);
            List<StudentIntentionView> Viewdata = new List<StudentIntentionView>();

            foreach (var item in data)
            {
                StudentIntentionView view = new StudentIntentionView();
                view.ID = item.ID;
                if (item.AreaID == null)
                {
                    view.AreaName = "其他城市";
                }
                else
                {
                    view.AreaName = dbemploymentAreas.GetEntity(item.AreaID).AreaName;
                }
                
                var tradata = dbproScheduleForTrainees.GetTraineesByStudentNumber(item.StudentNO);
                view.classnumnber = tradata.ClassID;
                var studentobj = dbproStudentInformation.GetEntity(item.StudentNO);
                view.Familyphone = studentobj.Familyphone;
                view.StudentNO = item.StudentNO;
                var stringlist = studentobj.Guardian.Split(',');
                view.RelativesName = stringlist[0].Replace(" ", "");
                view.Relationship = stringlist[1].Replace(" ", "");
                view.sex = studentobj.Sex == true ? "男" : "女";
                view.StudentName = studentobj.Name;
                view.Telephone = studentobj.Telephone;
                var studentDataKeepAndRecordobj = dbstudentDataKeepAndRecord.GetEntity(studentobj.StudentPutOnRecord_Id);
                view.StuSchoolName = studentDataKeepAndRecordobj.StuSchoolName;
                view.Salary = item.Salary;
                view.Nation = studentobj.Nation;
                view.Familyphone = studentobj.Familyphone;
                view.identitydocument = studentobj.identitydocument;
                Viewdata.Add(view);
            }
            var resultdata1 = Viewdata.OrderByDescending(a => a.StudentNO).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = Viewdata.Count(),
                data = resultdata1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改传入过来的意向id
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult uptstudnetintention(int param0)
        {

            dbstudentIntention = new StudentIntentionBusiness();
            dbproStudentInformation = new ProStudentInformationBusiness();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            dbemploymentAreas = new EmploymentAreasBusiness();
            var querydata = dbstudentIntention.GetEntity(param0);
            var stuobj = dbproStudentInformation.GetEntity(querydata.StudentNO);
            StudentIntentionView view = new StudentIntentionView();
            var undres = dbproScheduleForTrainees.GetTraineesByStudentNumber(querydata.StudentNO);
            view.classnumnber = undres.ClassID;
            var studentobj = dbproStudentInformation.GetEntity(querydata.StudentNO);
            view.StudentNO = stuobj.StudentNumber;
            view.StudentName = studentobj.Name;
            view.Telephone = studentobj.Telephone;
            view.Salary = querydata.Salary;

            

            view.AreaID = querydata.AreaID;
            view.ID = param0;
            ViewBag.querydata = Newtonsoft.Json.JsonConvert.SerializeObject(view);

            var aa = dbemploymentAreas.GetAll();
            var cc = aa.Select(a => new
            {
                a.AreaName,
                a.ID
            }).ToList();
            ViewBag.city = Newtonsoft.Json.JsonConvert.SerializeObject(cc);
            return View();

        }

        [HttpPost]
        /// <summary>
        /// 修改意向
        /// </summary>
        /// <returns></returns>
        public ActionResult upttyping(StudentIntentionView param0) {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbstudentIntention = new StudentIntentionBusiness();
                var StudnetIntentionobj = dbstudentIntention.GetEntity(param0.ID);

                if (StudnetIntentionobj != null)
                {
                    dbproStudentInformation = new ProStudentInformationBusiness();
                    var studentobj = dbproStudentInformation.GetStudent(param0.StudentNO);
                    if (studentobj.Telephone != param0.Telephone)
                    {
                        studentobj.Telephone = param0.Telephone;
                        dbproStudentInformation.Update(studentobj);
                    }
                    if (param0.AreaID != 0)
                    {
                        StudnetIntentionobj.AreaID = param0.AreaID;
                    }
                    else
                    {
                        StudnetIntentionobj.AreaID = null;
                    }
                    StudnetIntentionobj.Salary = param0.Salary;
                    dbstudentIntention.Update(StudnetIntentionobj);
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