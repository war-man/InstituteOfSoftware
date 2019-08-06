using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    public class StudentDataKeepController : Controller
    {
        // GET: Teaching/StudentDataKeep
        //数据备案主页面
        public ActionResult StudentDataKeepIndex()
        {
            return View();
        }
    }
}