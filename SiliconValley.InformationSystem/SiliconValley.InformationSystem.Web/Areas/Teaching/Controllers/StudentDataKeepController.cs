using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    public class StudentDataKeepController : BaseMvcController
    {
        // GET: Teaching/StudentDataKeep
        //这是一个数据备案的主页面

        public ActionResult StudentDataKeepIndex()
        {
            return View();
        }
    }
}