using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;

    [CheckLogin]
    public class TeacherController : Controller
    {
        // GET: Teaching/Teacher


        // 教员上下文
        private readonly TeacherBusiness db_teacher;
        private readonly EmployeesInfoManage db_emp;
        public TeacherController()
        {
            db_teacher = new TeacherBusiness();
            db_emp = new EmployeesInfoManage();
        }
        public ActionResult TeachersInfo()
        {

            return View();
        }

        public ActionResult TeacherData(int limit,int page)
        {


            var list = db_teacher.GetList().Skip((page -1) * limit).Take(limit);

            var returnlist = list.Select(x => new { TeacherID = x.TeacherID, TeacherName = x.TeachingExperience, WorkExperience = x.WorkExperience });

            var obj = new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = returnlist
            };

            return Json(obj,JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTeacherByID(int Id)
        {
            Teacher t = db_teacher.GetList().Where(t => t.TeacherID == Id).FirstOrDefault();

            return Json(t, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 对教员的操作的视图
        /// </summary>
        /// <param name="ID">教员Id</param>
        /// <returns>视图</returns>
        [HttpGet]
        public ActionResult Operating(int? Id)
        {
            return View();
        }

    }
}