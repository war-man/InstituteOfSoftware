using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    using SiliconValley.InformationSystem.Depository;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Business.Common;
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Business.ClassesBusiness;
    using SiliconValley.InformationSystem.Entity.Base_SysManage;

    public class RecordOfConversationController : Controller
    {


        private readonly ConversationBusiness db_conversation;
        private readonly TeacherClassBusiness db_teachingclass;
        private readonly TeacherBusiness db_teacher;



        public RecordOfConversationController()
        {
            db_teacher = new TeacherBusiness();
               db_teachingclass = new TeacherClassBusiness();
            db_conversation = new ConversationBusiness();
        }


        // GET: Teaching/RecordOfConversation
        public ActionResult ConversationIndex()
        {
           


            return View();
        }



        [HttpPost]
        public ActionResult GetConversationRecord(string begindate, string studentname)
        {

            AjaxResult result = new AjaxResult();

            List<ConversationRecordView> resultlist = new List<ConversationRecordView>();

            try
            {
                resultlist = db_conversation.GetScreenConversationRecord(begindate, studentname);

                result.ErrorCode = 200;
                result.Data = resultlist;
                result.Msg = "成功";


            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = resultlist;
                result.Msg = ex.Message;

            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetConversationrecords()
        {

            AjaxResult result = new AjaxResult();
            List<ConversationRecordView> resultlist = new List<ConversationRecordView>();
            try
            {
                var list = db_conversation.GetConversationRecords().OrderByDescending(d=>d.Time).ToList().Take(30);

               

                foreach (var item in list)
                {
                    var tempobj = db_conversation.GetConversationRecordView(item);

                    if (tempobj != null)
                        resultlist.Add(tempobj);
                }


                result.Data = resultlist;
                result.ErrorCode = 200;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.Data = resultlist;
                result.ErrorCode = 500;
                result.Msg = ex.Message;
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 添加访谈记录
        /// </summary>
        /// <returns></returns>
        public ActionResult Operations()
        {

            var resultlist = new List<ConversationRecordView>();

           var currentlogin = Base_UserBusiness.GetCurrentUser();

            //获取当前的登陆老师

            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == currentlogin.EmpNumber).ToList().FirstOrDefault();

            //获取我的班级
           var classlist = db_teachingclass.GetCrrentMyClass(teacher.TeacherID);

            ViewBag.myclass = classlist;

            return View();

        }


        /// <summary>
        /// 增加谈话记录
        /// </summary>
        /// <param name="recordView"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoAdd(ConversationRecordView recordView)
        {

            AjaxResult result = new AjaxResult();

            ConversationRecord record = new ConversationRecord();

            var currentlogin = Base_UserBusiness.GetCurrentUser();

            record.Content = recordView.Content;
            record.EmployeeId = currentlogin.EmpNumber;
            record.IsDel = false;
            record.Result = recordView.Result;
            record.StudenNumber = recordView.StudenNumber;
            record.Theme = recordView.Theme;
            record.Time = DateTime.Now;

            try
            {
                db_conversation.Insert(record);

                result.Data = null;
                result.ErrorCode = 200;
                result.Msg = "OK";

                BusHelper.WriteSysLog("添加数据",EnumType.LogType.添加数据);


            }
            catch (Exception ex)
            {

                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = ex.Message;

                BusHelper.WriteSysLog(ex.Message, EnumType.LogType.添加数据);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetStudentByClass(int classnumber)
        {
            

            ScheduleForTraineesBusiness db_studentclass = new ScheduleForTraineesBusiness();
           var list = db_studentclass.ClassStudent(classnumber);


            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查看谈话详细；记录
        /// </summary>
        /// <param name="id">谈话记录ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Detail(int id)
        {
            var record =  db_conversation.GetConversationRecords().Where(d=>d.ID== id).FirstOrDefault();

            var recordview = db_conversation.GetConversationRecordView(record);

            return View(recordview);

        }

        /// <summary>
        /// 获取学员的被访谈记录
        /// </summary>
        /// <param name="studentnumber"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetConversationRecordByStudentNumber(string studentnumber)
        {

            AjaxResult result = new AjaxResult();

            List<ConversationRecordView> list = new List<ConversationRecordView>();

            try
            {
                list = db_conversation.GetConversationRecordByStudentNumber(studentnumber);

                result.Data = list;
                result.ErrorCode = 200;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.Data = list;
                result.ErrorCode = 500;
                result.Msg = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

            
        }

    }
}