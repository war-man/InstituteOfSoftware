using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    [CheckLogin]
    public class StudentInformationController : Controller
    {
        // GET: Teachingquality/StudentInformation
        public ActionResult Index()
        {
            return View();
        }
    }
}