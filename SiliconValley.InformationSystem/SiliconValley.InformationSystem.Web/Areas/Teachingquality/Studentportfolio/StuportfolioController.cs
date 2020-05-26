using SiliconValley.InformationSystem.Business.StudentportfolioBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Studentportfolio
{
    public class StuportfolioController : Controller
    {
        private readonly StudentpoBusiness dbtext;

        public StuportfolioController(){
          dbtext=new StudentpoBusiness();
         }
        // GET: Teachingquality/Stuportfolio
        public ActionResult Index(string id)
        {
            ViewBag.studentid = id;
            return View();
        }

        public ActionResult Studentfine(string id)
        {
            return Json(dbtext.Studentfine(id));
        }

    }
}