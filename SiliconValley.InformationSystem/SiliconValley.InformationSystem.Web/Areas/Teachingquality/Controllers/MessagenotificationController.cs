using SiliconValley.InformationSystem.Business.Messagenotification_Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    public class MessagenotificationController : Controller
    {
        // GET: Teachingquality/Messagenotification
        public ActionResult Index()
        {
            return View();
        }

      
        //通知数据页面
        public ActionResult NoticeDate()
        {
            return View();
        }
        MessagenotificationBusiness messagenotificationBusiness = new MessagenotificationBusiness();
        public ActionResult Date(int page, int limit)
        {
            return Json(messagenotificationBusiness.Date(page, limit), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Release()
        {
            return View();
        }
    }
}