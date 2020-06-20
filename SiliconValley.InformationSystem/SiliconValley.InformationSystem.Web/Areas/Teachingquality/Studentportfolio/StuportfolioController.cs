using SiliconValley.InformationSystem.Business.StudentBusiness;
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
        public static string StudentID = null;
        private readonly StudentpoBusiness dbtext;

        public StuportfolioController(){
          dbtext=new StudentpoBusiness();
         }

        /// 学员信息
        StudentInformationBusiness studentInformation = new StudentInformationBusiness();
        // GET: Teachingquality/Stuportfolio
        public ActionResult Index(string id)
        {
            StudentID = id;
            ViewBag.studentid = id;
            return View();
        }

        public ActionResult Studentfine(string id)
        {
            return Json(dbtext.Studentfine(id));
        }
        //身份证照片
        [HttpGet]
        public ActionResult IDcardphoto()
        {

            return View(studentInformation.GetEntity(StudentID));
        }
        //学员访谈
        [HttpGet]
        public ActionResult Traineeinterview()
        {
            return View();
        }
        //家长访谈记录
        public ActionResult Parentinterview()
        {
            return View();
        }
    }
}