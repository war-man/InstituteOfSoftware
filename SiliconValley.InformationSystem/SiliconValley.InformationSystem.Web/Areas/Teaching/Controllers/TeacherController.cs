using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{

    using SiliconValley.InformationSystem.Business;
    public class TeacherController : Controller
    {
        // GET: Teaching/Teacher

           
        public ActionResult TeachersInfo()
        {
            return View();
        }
    }
}