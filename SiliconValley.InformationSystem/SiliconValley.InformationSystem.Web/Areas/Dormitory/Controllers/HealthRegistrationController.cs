using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Dormitory.Controllers
{
    /// <summary>
    /// 寝室卫生登记
    /// </summary>
    public class HealthRegistrationController : Controller
    {
        // GET: Dormitory/HealthRegistration
        public ActionResult HealthRegistrationIndex()
        {
            return View();
        }
    }
}