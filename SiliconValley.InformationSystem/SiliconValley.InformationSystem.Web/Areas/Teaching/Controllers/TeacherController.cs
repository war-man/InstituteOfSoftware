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
        public ActionResult TeachersInfo()
        {

            return View();
        }

        public ActionResult TeacherData()
        {

            Pagination pagination = new Pagination();
            pagination.page = 1;
            pagination.limit = 10;


            var teacherlist = db_teacher.GetPagination<Teacher>(db_teacher.GetIQueryable(), pagination);

            return null;
        }
    }
}