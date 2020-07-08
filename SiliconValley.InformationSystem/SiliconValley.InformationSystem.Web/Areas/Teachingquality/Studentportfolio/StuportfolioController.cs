using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.Cloudstorage_Business;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Business.StudentmanagementBusinsess;
using SiliconValley.InformationSystem.Business.StudentportfolioBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
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
        //对象存储业务类
        CloudstorageBusiness cloudstorage_Business = new CloudstorageBusiness();
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
        public ActionResult IDcardphoto(string id)
        {
            var student = studentInformation.GetEntity(id);
            student.Identitybackimg = cloudstorage_Business.ImagesFine("xinxihua", "IDcardphotoImg/Identitybackimg", student.Identitybackimg, 5);
            student.Identityjustimg = cloudstorage_Business.ImagesFine("xinxihua", "IDcardphotoImg/Identityjustimg", student.Identityjustimg, 5);
            return View(student);
        }
        // 学员家长访谈业务类型
        BaseBusiness<InterviewRecordsof> intervirereco = new BaseBusiness<InterviewRecordsof>();
        //学员访谈业务类
        InterviewStudentsBusiness interviewStudentsBusiness = new InterviewStudentsBusiness();
        //学员访谈
        [HttpGet]
        public ActionResult Traineeinterview(string id)
        {
            var studentid = id.Split(',');
            List<vierprice> listvier = new List<vierprice>();
            List<object> intss = new List<object>();
            if (studentid[0]=="1")
            {
               
                var x = interviewStudentsBusiness.GetList().Where(a => a.IsDelete == false && a.StudentNumberID == studentid[1]).OrderByDescending(a => a.Dateofinterview).ToList();
                foreach (var item in x)
                {
                    intss.Add(Convert.ToDateTime(item.Dateofinterview).ToLongDateString().ToString());
                }
                var myinterview = intss.Distinct().ToList();
                foreach (var item in myinterview)
                {
                    vierprice vierprice = new vierprice();
                    vierprice.Date = item.ToString();
                    foreach (var item1 in x)
                    {

                        string date = Convert.ToDateTime(item1.Dateofinterview).ToLongDateString().ToString();
                        if (item.ToString() == date)
                        {
                            vierprice vierprice1 = new vierprice();
                            vierprice1.GrandName = item1.InterviewTopics;//标题
                            vierprice1.Rategory = item1.Interviewcontent;//内容
                            vierprice.Chicked.Add(vierprice1);
                        }

                    }
                    listvier.Add(vierprice);
                }
            }
            else
            {
                var x = intervirereco.GetList().Where(a => a.IsDelete == false && a.Studentnumber == studentid[1]).OrderByDescending(a => a.Interviewtime).ToList();
                foreach (var item in x)
                {
                    intss.Add(Convert.ToDateTime(item.Interviewtime).ToLongDateString().ToString());
                }
                var myinterview = intss.Distinct().ToList();
                foreach (var item in myinterview)
                {
                    vierprice vierprice = new vierprice();
                    vierprice.Date = item.ToString();
                    foreach (var item1 in x)
                    {

                        string date = Convert.ToDateTime(item1.Interviewtime).ToLongDateString().ToString();
                        if (item.ToString() == date)
                        {
                            vierprice vierprice1 = new vierprice();
                            vierprice1.GrandName = item1.InterviewTopics;//标题
                            vierprice1.Rategory = item1.Interviewcontent;//内容
                            vierprice.Chicked.Add(vierprice1);
                        }

                    }
                    listvier.Add(vierprice);
                }
            }
         

            ViewBag.listvier = listvier;
            return View();
        }
        //家长访谈记录
        public ActionResult Parentinterview()
        {
         
            return View();
        }
    }
}