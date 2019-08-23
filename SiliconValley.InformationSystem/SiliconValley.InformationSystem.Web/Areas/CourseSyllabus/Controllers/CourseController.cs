using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.CourseSyllabus.Controllers
{
    using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Util;

    using SiliconValley.InformationSystem.Business.Common;

    [CheckLogin]
    public class CourseController : Controller
    {
        private readonly CourseBusiness db_course;
        private readonly SpecialtyBusiness db_major;
        private readonly GrandBusiness db_grand;
        private readonly CourseTypeBusiness db_coursetype;
        public CourseController()
        {
            this.db_course = new CourseBusiness();
            this.db_major = new SpecialtyBusiness();
            this.db_grand = new GrandBusiness();
            this.db_coursetype = new CourseTypeBusiness();
        }



        // GET: CourseSyllabus/Course
        public ActionResult CourseIndex()
        {
            return View();
        }


        /// <summary>
        /// 获取所有的课程数据
        /// </summary>
        /// <returns>课程json数据</returns>
        public ActionResult GetCourseData(int limit, int page)
        {
            var list = db_course.GetCurriculas();


            List<CourseView> resultlist = new List<CourseView>();

            foreach (var item in list)
            {
                resultlist.Add(db_course.ToCourseView(item));
            }

            var count = resultlist.Count;

            var objresult = new
            {
                code = 0,
                msg = "",
                count = count,
                data = resultlist.Skip((page - 1) * limit).Take(limit)
            };


            return Json(objresult, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 课程操作视图
        /// </summary>
        /// <param name="courseID">课程ID</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult OperationView(int? id)
        {
            CourseView curriculum = new CourseView();

            if (id != null)
            {
                curriculum = db_course.ToCourseView(db_course.GetCurriculas().Where(d => d.CurriculumID == id).FirstOrDefault());
            }

            //获取专业集合
            ViewBag.majors = db_major.GetSpecialties().Select(d => new SelectListItem() { Text = d.SpecialtyName, Value = d.Id.ToString() });

            //获取阶段集合
            ViewBag.grands = db_grand.GetList().Where(d => d.IsDelete == false).ToList().Select(d => new SelectListItem() { Text = d.GrandName, Value = d.Id.ToString() });

            //获取课程类型集合
            ViewBag.courseTypes = db_coursetype.GetCourseTypes().Select(d => new SelectListItem() { Text = d.TypeName, Value = d.Id.ToString() });

            return View(curriculum);

        }


        /// <summary>
        /// 课程的操作（添加 修改）
        /// </summary>
        /// <param name="courseView"></param>
        /// <param name="Grand"></param>
        /// <param name="Major"></param>
        /// <param name="CourseType"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DoOperation(CourseView courseView, int Grand, int Major, int CourseType)
        {

            AjaxResult result = new AjaxResult();

            courseView.Major = db_major.GetSpecialtyByID(Major);
            courseView.Grand = db_grand.GetList().Where(d => d.Id == Grand && d.IsDelete == false).FirstOrDefault();
            courseView.CourseType = db_coursetype.GetList().Where(d => d.IsDelete == false && d.Id == CourseType).FirstOrDefault();

            var course = db_course.ToCurriculum(courseView);
            if (courseView.CurriculumID == 0)
            {
               
                try
                {
                    this.DoAdd(course);
                    result.ErrorCode = 200;
                    result.Msg = "成功";


                    BusHelper.WriteSysLog("新增课程", Entity.Base_SysManage.EnumType.LogType.添加数据);

                }
                catch (Exception ex)
                {
                    result.Data = null;
                    result.ErrorCode = 500;
                    result.Msg = "失败";

                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
            }
            else
            {
                try
                {
                    this.DoEdit(course);
                    result.ErrorCode = 200;
                    result.Msg = "成功";
                    BusHelper.WriteSysLog("修改课程", Entity.Base_SysManage.EnumType.LogType.编辑数据);
                }
                catch (Exception ex)
                {
                    result.Data = null;
                    result.ErrorCode = 500;
                    result.Msg = "失败";
                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void DoAdd(Curriculum curriculum)
        {
            db_course.DoAdd(curriculum);

        }


        public void DoEdit(Curriculum curriculum)
        {
            db_course.DoEdit(curriculum);

        }


        /// <summary>
        /// 课程详细页面 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult DetailView(int id)
        {

            //获取专业集合
            ViewBag.majors = db_major.GetSpecialties().Select(d => new SelectListItem() { Text = d.SpecialtyName, Value = d.Id.ToString() });

            //获取阶段集合
            ViewBag.grands = db_grand.GetList().Where(d => d.IsDelete == false).ToList().Select(d => new SelectListItem() { Text = d.GrandName, Value = d.Id.ToString() });

            //获取课程类型集合
            ViewBag.courseTypes = db_coursetype.GetCourseTypes().Select(d => new SelectListItem() { Text = d.TypeName, Value = d.Id.ToString() });


            var course =  db_course.GetCurriculas().Where(d=>d.CurriculumID==id).FirstOrDefault();

            var courseView = db_course.ToCourseView(course);

          return View(courseView);
        }


    }
}