using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.StudentManeger;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{  //学员信息模块
    [CheckLogin]
    public class StudentInformationController : Controller
    {
        public class Student { }

        private readonly StudentInformationManeger dbtext;
        public StudentInformationController()
        {
            dbtext = new StudentInformationManeger();
            
        }
        // GET: Teachingquality/StudentInformation
        public ActionResult Index()
        {
            return View();
        }
        //学员注册
        public ActionResult Registeredtrainees()
        {
           var x= dbtext.GetList();
            return View();
        }
    }
}