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
    [CheckLogin]
    public class TeacherController : Controller
    {
        // GET: Teaching/Teacher


        // 教员上下文
        private readonly TeacherBusiness db_teacher;

        public TeacherController()
        {
            db_teacher = new TeacherBusiness();
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


        /// <summary>
        /// 对教员的操作的视图
        /// </summary>
        /// <param name="ID">教员Id</param>
        /// <returns>视图</returns>
        [HttpGet]
        public ActionResult Operating(int? ID)
        {
            return View();
        }




    }
}