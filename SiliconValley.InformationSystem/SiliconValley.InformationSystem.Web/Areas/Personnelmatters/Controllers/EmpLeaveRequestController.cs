using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    public class EmpLeaveRequestController : Controller
    {
        // GET: Personnelmatters/EmpLeaveRequest
        public ActionResult LeaveIndex()
        {
            return View();
        }
    }
}