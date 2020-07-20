using Newtonsoft.Json;
using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.Cloudstorage_Business;
using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.ExaminationSystemBusiness;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Business.StudentmanagementBusinsess;
using SiliconValley.InformationSystem.Business.StudentportfolioBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Studentportfolio
{
    //学员信息模块
    [CheckLogin]
    [CheckUrlPermission]
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
            //student.Identitybackimg = cloudstorage_Business.text("xinxihua", "IDcardphotoImg/Identitybackimg/"+student.Identitybackimg);
            //student.Identityjustimg = cloudstorage_Business.text("xinxihua", "IDcardphotoImg/Identityjustimg/"+ student.Identityjustimg);
            return View(student);
        }
        // 学员家长访谈业务类型
        BaseBusiness<InterviewRecordsof> intervirereco = new BaseBusiness<InterviewRecordsof>();
        //学员访谈业务类
        InterviewStudentsBusiness interviewStudentsBusiness = new InterviewStudentsBusiness();
        //学员访谈/家长访谈
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
        //保险记录
        public ActionResult Parentinterview(string id)
        {
            BaseBusiness<DetailedStudentIn> DetailedStudentInBusiness = new BaseBusiness<DetailedStudentIn>();
          ViewBag.DetailedStudentIn= DetailedStudentInBusiness.GetList().Where(a => a.StudentID == id).ToList();
            return View();
        }
        /// <summary>
        /// 成绩
        /// </summary>
        /// <returns></returns>
        public ActionResult Achievement(string id)
        {
            ExamScoresBusiness examScoresBusiness = new ExamScoresBusiness();
         ViewBag.examScoresBusiness= examScoresBusiness.StudentScores(id);
            return View();
        }
        /// <summary>
        /// 缴费信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Paymentinformation(string id)
        {
            StudentFeeStandardBusinsess studentFeeStandardBusinsess = new StudentFeeStandardBusinsess();
            ViewBag.vier = studentFeeStandardBusinsess.FienPrice(id);
            ViewBag.Tuitionrefund = studentFeeStandardBusinsess.FienTuitionrefund(studentFeeStandardBusinsess.FienPrice(id));
            return View();
        }
        /// <summary>
        /// 居住信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Dormitoryinformation(string id)
        {
            object tempdata = new object();
            BaseBusiness<Tung> dbtung = new BaseBusiness<Tung>();
            BaseBusiness<Dormitoryfloor> dbfloor = new BaseBusiness<Dormitoryfloor>();
            BaseBusiness<TungFloor> dbtungfloor = new BaseBusiness<TungFloor>();
            BaseBusiness<BenNumber> dbbed = new BaseBusiness<BenNumber>();
           
            AccdationinformationBusiness accdationinformationBusiness = new AccdationinformationBusiness();
            var TungFloor = accdationinformationBusiness.GetDormBystudentno(id);
            var roomNumber = TungFloor.DormInfoName;//房间号码
            var Tungfloorobj = dbtungfloor.GetList().Where(d => d.Id == TungFloor.TungFloorId).FirstOrDefault();
            var tungobj = dbtung.GetList().Where(d => d.Id == Tungfloorobj.TungId).FirstOrDefault(); //栋对象
            var floorobj = dbfloor.GetList().Where(d => d.ID == Tungfloorobj.FloorId).FirstOrDefault();//楼层对象
            var accdation = accdationinformationBusiness.GetAccdationByStudentNumber(id);
            var bedName = dbbed.GetList().Where(d => d.Id == accdation.BedId).FirstOrDefault().BenNo;//床位号
            if (TungFloor == null)
            {
                 tempdata = new
                {
                    TungInfo = "无",
                    FloorInfo = "无",
                    RoomInfo = "无",
                    BedInfo = "无",

                };
            }
            else
            {
                tempdata = new
                {
                    TungInfo = tungobj.TungName,
                    FloorInfo = floorobj.FloorName,
                    RoomInfo = roomNumber,
                    BedInfo = bedName

                };
            }


            ViewBag.tempdata = JsonConvert.SerializeObject(tempdata);
            return View();
        }
      
    }
}