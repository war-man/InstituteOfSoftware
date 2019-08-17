using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.StudentmanagementBusinsess;
using SiliconValley.InformationSystem.Business.StageGrade_Business;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    public class StudentAttendancController : Controller
    {
        private readonly StudentAttendanceBusiness dbtext;
        public StudentAttendancController()
        {
            dbtext = new StudentAttendanceBusiness();

        }
        //学员表
        StudentInformationBusiness student = new StudentInformationBusiness();
        //员工表
        EmployeesInfoManage infoBusiness = new EmployeesInfoManage();
        //学员班级表
        ScheduleForTraineesBusiness Stuclass = new ScheduleForTraineesBusiness();
        // GET: Teachingquality/StudentAttendanc
        public ActionResult Index()
        {
            return View();

       }
        //获取数据
        public ActionResult GetDate(int page, int limit)
        {
            //
            List<StudentAttendance> list =dbtext.GetList();

            var listx = list.Select(a => new
            {
                InspectionDate=a.InspectionDate,
                Attendancestatus=a.Attendancestatus,
                Registrar=infoBusiness.GetEntity(a.Registrar).EmployeeId,
                AttendanceTitle=a.AttendanceTitle,
                StudentID=a.StudentID,
                Remarks=a.Remarks,
                Stuclass= Stuclass.GetList().Where(q=>q.StudentID==a.StudentID&&q.CurrentClass==true).First().ClassID,  //班级
                Name = student.GetEntity(a.StudentID).Name,   //姓名
                RegistrarName= infoBusiness.GetEntity(a.Registrar).EmpName //登记人姓名
            }).ToList();
            var dataList = listx.OrderByDescending(a => a.InspectionDate).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = dataList.Count,
                data = dataList
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //登记出勤
        [HttpGet]
        public ActionResult Registerattendance()
        {
            return View();
        }
        [HttpPost]
        //登记出勤更新数据
        public ActionResult Registerattendance(StudentAttendance studentAttendanc)
        {
            //多个学号组成后面截取逗号
           string tu= studentAttendanc.StudentID.Substring(0, studentAttendanc.StudentID.Length - 1);
            //转成数组
            // Session["EmpNumber"].ToString();
            string EmpNumber = "201908150001";
            string[] stu= tu.Split(',');
            AjaxResult result = null;
            try
            {
                //if (string.IsNullOrEmpty(EmpNumber))
                //{
                //    EmpNumber = "201908150001";
                //}
                //
                List< StudentAttendance> list = new List<StudentAttendance>();
                for (int i = 0; i < stu.Length; i++)
                {
                    StudentAttendance student = new StudentAttendance();
                    student.AttendanceTitle = studentAttendanc.AttendanceTitle;
                    student.Remarks = studentAttendanc.Remarks;
                    student.Attendancestatus = studentAttendanc.Attendancestatus;
                    student.InspectionDate = studentAttendanc.InspectionDate;
                    student.StudentID = stu[i];
                    student.Addtime = DateTime.Now;
                    student.IsDelete = false;
                    student.Registrar = EmpNumber;
                    list.Add(student);
                }

              
                    
                
                dbtext.BulkInsert(list);
                result = new SuccessResult();
                result.Msg = "记录成功";
                result.Success = true;
            }
            catch (Exception)
            {

                result = new ErrorResult();
                result.ErrorCode = 500;
                result.Success = false;
                result.Msg = "服务器错误";
            }
           
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        //查询数据库是否有相同数据
        public ActionResult Errnt(StudentAttendance studentAttendanc)
        {
          
            string tu = studentAttendanc.StudentID.Substring(0, studentAttendanc.StudentID.Length - 1);
            //转成数组
            string[] stu = tu.Split(',');
            var list = dbtext.GetList();
            List<StudentAttendance> mylist = new List<StudentAttendance>();
            for (int i = 0; i < stu.Length; i++)
            {

              var count=  list.Where(a => a.StudentID == stu[i] && a.InspectionDate == studentAttendanc.InspectionDate && a.Attendancestatus == studentAttendanc.Attendancestatus && a.AttendanceTitle == studentAttendanc.AttendanceTitle).ToList();
                if (count.Count>0)
                {
                    mylist.AddRange(count);
                }
            }
       
            return Json(mylist.Select(a => new { Name = student.GetEntity(a.StudentID).Name, StudentID = a.StudentID }), JsonRequestBehavior.AllowGet);
               
       
        }
        //按班级查询学员，并可选择学员
        public ActionResult StuAttendance()
        {
            string StudentID = Request.QueryString["StudentID"];
            ViewBag.Classe=StudentID;
            //1710NA
            var classStu = Stuclass.GetList().Where(a => a.CurrentClass == true && a.ClassID == StudentID).ToList();

            List<StudentInformation> list = new List<StudentInformation>();
            foreach (var item in classStu)
            {
                list.Add(student.GetEntity(item.StudentID));
            }
           
           
            return View(list);
        }
        //查询班级学员表
        public ActionResult stuclassSelete()
        {
            string StudentID = Request.QueryString["StudentID"];
          
            //1710NA
            var classStu = Stuclass.GetList().Where(a => a.CurrentClass == true && a.ClassID == StudentID).ToList();
            return Json(classStu, JsonRequestBehavior.AllowGet);
        }
    }
}