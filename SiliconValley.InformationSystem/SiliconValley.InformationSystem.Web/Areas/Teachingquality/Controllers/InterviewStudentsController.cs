using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Business.StudentmanagementBusinsess;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    public class InterviewStudentsController : Controller
    {
        // GET: Teachingquality/InterviewStudents
        //学员班级表
        ScheduleForTraineesBusiness Stuclass = new ScheduleForTraineesBusiness();
        //学员表
        StudentInformationBusiness student = new StudentInformationBusiness();
        //员工表
        EmployeesInfoManage infoBusiness = new EmployeesInfoManage();
        //班级表
        ClassScheduleBusiness classschedu = new ClassScheduleBusiness();
        //实例化
        private readonly InterviewStudentsBusiness dbtext;
        public InterviewStudentsController()
        {
            dbtext = new InterviewStudentsBusiness();
        }
        //主页面
        public ActionResult Index()
        {
            return View();
        }
        //主页面获取表格数据
        public ActionResult GetDate(int page, int limit)
        {
            var list = dbtext.GetList();
            var mylist = list.Select(a => new
            {
                ID=a.ID,
                InterviewTopics = a.InterviewTopics, //谈话标题
                Interviewcontent = a.Interviewcontent, //谈话内容
                StudentNumberID = a.StudentNumberID,//谈话学员学号
                StudentNumberName = student.GetEntity(a.StudentNumberID).Name,//谈话学员姓名
                StuClass = Stuclass.GetList().Where(c => c.StudentID == a.StudentNumberID && c.CurrentClass == true).FirstOrDefault().ClassID, //谈话学员班级
                InterviewRecorderID = a.InterviewRecorderID, //记录人编号
                InterviewRecorderName = infoBusiness.GetEntity(a.InterviewRecorderID).EmpName, //姓名
                Dateofinterview = a.Dateofinterview //谈话日期
            });
            var dataList = mylist.OrderBy(a => a.ID).Skip((page - 1) * limit).Take(limit).ToList();
          
            var data = new
            {
                code = "",
                msg = "",
                count = dataList.Count,
                data = dataList
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //添加 编辑学员谈话记录
        public ActionResult StudIenterEntiy()
        {
          
            ViewBag.Name = "登记学员访谈数据";
            return View();
        }
        }
}