using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Business.ClassesBusiness;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Util;

    [CheckLogin]
    public class ClassController : Controller
    {
        // GET: Teaching/Class


        private readonly TeacherClassBusiness db_teacherclass;
        private readonly TeacherBusiness db_teacher;
        private readonly StuHomeWorkBusiness db_homework;

        public ClassController()
        {
            db_homework = new StuHomeWorkBusiness();
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



        /// <summary>
        ///获取班级详细信息
        /// </summary>
        /// <param name="classnumber"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetClassInfo(string classnumber)
        {

            AjaxResult result = new AjaxResult();

            try
            {
                var temp1 = db_teacherclass.GetClassByClassNumber(classnumber);
               var obj = db_teacherclass.GetClassTableView(temp1);

                result.Data = obj;
                result.ErrorCode = 200;
                result.Msg="成功";

            }
            catch (Exception ex)
            {

                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = ex.Message;
            }


            return Json(result, JsonRequestBehavior.AllowGet);
            


        }
        

        /// <summary>
        /// 查看学生作业提交情况
        /// </summary>
        /// <param name="studentnumber">学员编号</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult StuHomeWorkSubmission(string studentnumber)
        {

            ViewBag.studentnumber = studentnumber;

            return View();
        }


        /// <summary>
        /// 记录学生作业的未完成记录
        /// </summary>
        /// <param name="studentnumber"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RecordStuHomeWorkSubmission(string studentnumber)
        {
           StudentInformation student = db_teacherclass.GetStudentByNumber(studentnumber);


            return View(student);
        }


        /// <summary>
        /// 记录学生作业的未完成记录
        /// </summary>
        /// <param name="studentnumber"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RecordStuHomeWorkSubmission(HomeWorkFinishRate homeWork)
        {
            AjaxResult result = new AjaxResult();

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            var teacher = db_teacher.GetTeachers().Where(d => d.EmployeeId == user.EmpNumber).FirstOrDefault();

            var ClassroomHomeWork = Request.Form["ClassroomHomeWork"];
           var AfterClassHomeWork = Request.Form["AfterClassHomeWork"];

            homeWork.AfterClassHomeWork = AfterClassHomeWork == "on" ? false : true;
            homeWork.ClassroomHomeWork = ClassroomHomeWork == "on" ? false : true;
            homeWork.CheckDate = DateTime.Now;
            homeWork.ChekTeacher = teacher.TeacherID;
            homeWork.IsDel = false;

            try
            {
                //首先判断是否重复记录

                if (db_homework.GetHomeWorkFinishRates().Where(d => d.StudentNumber == homeWork.StudentNumber && d.ReleaseDate == homeWork.ReleaseDate).FirstOrDefault() == null)
                {
                    db_homework.Insert(homeWork);

                    result.Data = null;
                    result.ErrorCode = 200;
                    result.Msg = "成功";
                }
                else
                {
                    result.Data = null;
                    result.ErrorCode = 501;
                    result.Msg = "失败";
                }


              


                Base_UserBusiness.WriteSysLog("成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.ErrorCode = 500;
                result.Msg = ex.Message;

                Base_UserBusiness.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }


            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetStuHomeWorkStatus(string studentnumber, string Month)
        {

            AjaxResult result = new AjaxResult();

            List<StuHomeWorkView> resultlist = new List<StuHomeWorkView>();
            try
            {
                DateTime date = DateTime.Parse(Month);

                var temp1 = db_homework.GetHomeWorkFinishRates().Where(d => d.StudentNumber == studentnumber).ToList();


                foreach (var item in temp1)
                {
                    DateTime date1 = item.ReleaseDate;

                    if (date.Year == date1.Year && date.Month == date1.Month)
                    {

                        //转为视图模型
                        StuHomeWorkView workView = new StuHomeWorkView();
                        workView.ID = item.ID;
                        workView.IsDel = item.IsDel;
                        workView.Notes = item.Notes;
                        workView.ReleaseDate = item.ReleaseDate;
                        workView.StudentNumber = item.StudentNumber;
                        workView.AfterClassHomeWork=item.AfterClassHomeWork==true?"完成":"未完成";
                        workView.ClassroomHomeWork= item.ClassroomHomeWork == true ? "完成" : "未完成";


                        resultlist.Add(workView);
                    }

                }

                result.ErrorCode = 200;
                result.Data = resultlist;
                result.Msg="成功";


            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = resultlist;
                result.Msg = ex.Message;
            }

          

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取班级班干部
        /// </summary>
        /// <param name="classnumber"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult GetClassCadres(string classnumber)
        {

            AjaxResult result = new AjaxResult();

            Dictionary<string, StudentInformation> list = new Dictionary<string, StudentInformation>();

            try
            {
                list =  db_teacherclass.GetClassCadres(classnumber);

                result.Data = list;
                result.ErrorCode = 200;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.Data = list;
                result.ErrorCode = 500;
                result.Msg =ex.Message ;
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetStuHomeWorkDetail(int id)
        {
           var obj = db_homework.GetList().Where(d => d.IsDel == false && d.ID == id).FirstOrDefault(); ;


            //转为视图模型
            StuHomeWorkView workView = new StuHomeWorkView();
            workView.ID = obj.ID;
            workView.IsDel = obj.IsDel;
            workView.Notes = obj.Notes;
            workView.ReleaseDate = obj.ReleaseDate;
            workView.StudentNumber = obj.StudentNumber;
            workView.AfterClassHomeWork = obj.AfterClassHomeWork == true ? "完成" : "未完成";
            workView.ClassroomHomeWork = obj.ClassroomHomeWork == true ? "完成" : "未完成";
            workView.StudentName = db_teacherclass.GetStudentByNumber(obj.StudentNumber).Name;


            return View(workView);
        }


    }
}