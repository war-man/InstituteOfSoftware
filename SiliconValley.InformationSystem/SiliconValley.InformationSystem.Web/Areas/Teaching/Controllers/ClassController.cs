using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Business.ClassesBusiness;
    public class ClassController : Controller
    {
        // GET: Teaching/Class


        private readonly TeacherClassBusiness db_teacherclass;
        private readonly TeacherBusiness db_teacher;

        public ClassController()
        {
            db_teacherclass = new TeacherClassBusiness();
            db_teacher = new TeacherBusiness();
        }

        public ActionResult Index()
        {

            //提供我的班级列表

            //获取当前登陆的教员

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

             var teacher = db_teacher.GetTeachers().Where(d=>d.EmployeeId==user.EmpNumber).FirstOrDefault();

            var myclasslist =  db_teacherclass.GetCrrentMyClass(teacher.TeacherID);

            List<ClassTableView> resultlist = new List<ClassTableView>();

            foreach (var item in myclasslist)
            {

               var temp = db_teacherclass.GetClassTableView(item);

                if (temp != null)
                    resultlist.Add(temp);
            }


            ViewBag.classlist = resultlist;


            return View();
        }


        /// <summary>
        /// 获取班级学员
        /// </summary>
        /// <param name="classnumber">编辑编号</param>
        /// <returns></returns>
        public ActionResult GetStudentByClass(string classnumber)
        {

            ScheduleForTraineesBusiness scheduleForTraineesBusiness = new ScheduleForTraineesBusiness();

            var list = scheduleForTraineesBusiness.ClassStudent(classnumber);


            List<StudentDetailView> resultlist = new List<StudentDetailView>();

            foreach (var item in list)
            {
               var temp = db_teacherclass.GetStudetentDetailView(item);

                if (temp != null)
                    resultlist.Add(temp);
            }

            return Json(resultlist, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 学生详细页面
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult StudentDetailInfo(string studentnumber)
        {

           var student = db_teacherclass.GetStudentByNumber(studentnumber);

           var teacher = db_teacherclass.GetStudetentDetailView(student);

            return View(teacher);

        }


        

    }
}