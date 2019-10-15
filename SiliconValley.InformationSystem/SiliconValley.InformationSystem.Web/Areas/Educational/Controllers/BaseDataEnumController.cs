using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    public class BaseDataEnumController : Controller
    {
        // GET: Educational/BaseDataEnum
        public ActionResult BaseDataEnumIndexView()
        {
            return View();
        }

        public ActionResult GetBaseDataEnumData()
        {
            return null;
        }
    }
}