using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    [CheckLogin]
    public class ConvertCourseController : Controller
    {
        // GET: /Educational/ConvertCourse/ConvertCourseIndexView
        public ActionResult ConvertCourseIndexView()
        {
            return View();
        }

        public ActionResult ConvertTableData(int limit,int page)
        {
            return View();
        }
    }
}