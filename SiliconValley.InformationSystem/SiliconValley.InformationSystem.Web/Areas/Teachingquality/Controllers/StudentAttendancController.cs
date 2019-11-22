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
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    //出勤
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

        //班级表
        ClassScheduleBusiness classSchedule = new ClassScheduleBusiness();
        // GET: Teachingquality/StudentAttendanc
        public ActionResult Index()
        {
        
            ViewBag.ClassName = classSchedule.GetList().Select(a => new SelectListItem { Text = a.ClassNumber, Value = a.ClassNumber }).ToList();
            return View();

       }
        //获取数据
      
        public ActionResult GetDate(int page, int limit,string Name,string Attendancestatus,string qBeginTime,string identitydocument,string qEndTime,string ClassName)
        {

         
            try
            {  List<StudentAttendance> list = new List<StudentAttendance>();
                List<StudentAttendance> lists = new List<StudentAttendance>();
                if (!string.IsNullOrEmpty(ClassName))
                {
                    int ClassNames = Convert.ToInt32(ClassName);
                    var it = classSchedule.ClassStudentneList(ClassNames);
                    foreach (var item in it)
                    {
                        list.AddRange(dbtext.GetList().Where(a => a.StudentID == item.StuNameID).ToList());
                    }
                }
            if (!string.IsNullOrEmpty(Name))
            {
                var stu = student.GetList().Where(a => a.Name.Contains(Name)).ToList();
                  
                           foreach (var item in stu)
                            {    
                            // list = list.Distinct().ToList();
                            if (!string.IsNullOrEmpty(ClassName))
                           { lists.AddRange(list.Where(a => a.StudentID == item.StudentNumber)); }
                            else { lists.AddRange(dbtext.GetList().Where(a => a.StudentID == item.StudentNumber)); }
                            }
                        list = lists;

                    //.Mylist("StudentAttendance");

                }
                else if(string.IsNullOrEmpty(Name)&&string.IsNullOrEmpty(ClassName)) { list = dbtext.GetList(); }
           
            if (!string.IsNullOrEmpty(Attendancestatus))
            {
                list = list.Where(a => a.Attendancestatus==Attendancestatus).ToList();
            }
            if (!string.IsNullOrEmpty(qBeginTime))
            {
                list = list.Where(a => a.InspectionDate >= Convert.ToDateTime(qBeginTime)).ToList();
            }
            if (!string.IsNullOrEmpty(qEndTime))
            {
                list = list.Where(a => a.InspectionDate <= Convert.ToDateTime(qEndTime)).ToList();
            }
            if (!string.IsNullOrEmpty(identitydocument))
            {
                list = list.Where(a => a.StudentID== identitydocument).ToList();
            }

            var listx = list.Select(a => new
            {
                InspectionDate=a.InspectionDate,
                Attendancestatus=a.Attendancestatus,
                Registrar=a.Registrar,
                AttendanceTitle=a.AttendanceTitle,
                StudentID=a.StudentID,
                Remarks=a.Remarks,
                Stuclass= Stuclass.GetList().Where(q=>q.StudentID==a.StudentID&&q.CurrentClass==true).FirstOrDefault().ClassID,  //班级
                Name = student.GetEntity(a.StudentID).Name,   //姓名
                RegistrarName= infoBusiness.GetEntity(a.Registrar).EmpName //登记人姓名
            }).ToList();
            var dataList = listx.OrderByDescending(a => a.InspectionDate).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = listx.Count,
                data = dataList
            };
          return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.加载数据);
                return Json("数据异常", JsonRequestBehavior.AllowGet);
                throw;
            }
            
        }



        //饼状图
        public ActionResult MyDate(string Name,  string qBeginTime, string identitydocument, string qEndTime, string ClassName)
        {
            List<StudentAttendance> list = new List<StudentAttendance>();
            List<StudentAttendance> lists = new List<StudentAttendance>();
            List<PiechartView> stulist = new List<PiechartView>();
            if (!string.IsNullOrEmpty(ClassName))
            {
                int ClassNames = int.Parse(ClassName);
                var it = classSchedule.ClassStudentneList(ClassNames);
                foreach (var item in it)
                {
                    list.AddRange(dbtext.GetList().Where(a => a.StudentID == item.StuNameID).ToList());
                }
            }
            if (!string.IsNullOrEmpty(Name))
            {
                var stu = student.GetList().Where(a => a.Name.Contains(Name)).ToList();

                foreach (var item in stu)
                {
                    // list = list.Distinct().ToList();
                    if (!string.IsNullOrEmpty(ClassName))
                    { lists.AddRange(list.Where(a => a.StudentID == item.StudentNumber)); }
                    else { lists.AddRange(dbtext.GetList().Where(a => a.StudentID == item.StudentNumber)); }
                }
                list = lists;



            }
            else if (string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(ClassName)) { list = dbtext.Mylist("StudentAttendance"); }
          
            if (!string.IsNullOrEmpty(qBeginTime))
            {
                list = list.Where(a => a.InspectionDate >= Convert.ToDateTime(qBeginTime)).ToList();
            }
            if (!string.IsNullOrEmpty(qEndTime))
            {
                list = list.Where(a => a.InspectionDate <= Convert.ToDateTime(qEndTime)).ToList();
            }
            if (!string.IsNullOrEmpty(identitydocument))
            {
                list = list.Where(a => a.StudentID == identitydocument).ToList();
            }

            PiechartView piechartViews = new PiechartView();
            piechartViews.count = list.Where(a => a.Attendancestatus == "早退").Count();
            piechartViews.showname = "早退";
            piechartViews.Corlor = "#CC00CC";
            stulist.Add(piechartViews);
          
            PiechartView piechartView = new PiechartView();
            piechartView.count = list.Where(a => a.Attendancestatus == "迟到").Count();
             piechartView.showname = "迟到";
             piechartView.Corlor = "#AAAAAA";
            stulist.Add(piechartView);
            PiechartView piechartViews1 = new PiechartView();
            piechartViews1.count = list.Where(a => a.Attendancestatus == "缺勤").Count();
            piechartViews1.showname = "缺勤";
            piechartViews1.Corlor = "#FF0000";
            stulist.Add(piechartViews1);
            PiechartView piechartViews2 = new PiechartView();
            piechartViews2.count = list.Where(a => a.Attendancestatus == "请假").Count();
            piechartViews2.showname = "请假";
            piechartViews2.Corlor = "#00FF00 ";
            stulist.Add(piechartViews2);
            return Json(stulist, JsonRequestBehavior.AllowGet);


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
                dbtext.Remove("StudentAttendance");
                BusHelper.WriteSysLog("出勤记录成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {

                result = new ErrorResult();
                result.ErrorCode = 500;
                result.Success = false;
                result.Msg = "服务器错误";
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            
            }
           
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        //查询数据库是否有相同数据
        public ActionResult Errnt(StudentAttendance studentAttendanc)
        {
          
            string tu = studentAttendanc.StudentID.Substring(0, studentAttendanc.StudentID.Length - 1);
            //转成数组
            string[] stu = tu.Split(',');
            var list = dbtext.Mylist("StudentAttendance");
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
            string InteStu = Request.QueryString["InteStu"];
            string UnName = Request.QueryString["UnName"];
            if (!string.IsNullOrEmpty(InteStu))
            {
                ViewBag.InteStu = InteStu;
            }
            else
            {
                ViewBag.InteStu = "";
            }
            ViewBag.Classe=StudentID;
            //    ViewBag.InteStu = "";
            //1710NA
            if (!string.IsNullOrEmpty(UnName))
            {
                StudentUnionBusiness studentUnionBusiness = new StudentUnionBusiness();
                return View(studentUnionBusiness.StudentList(StudentID));
            }
            else
            {
                var classStu = Stuclass.GetList().Where(a => a.CurrentClass == true && a.ClassID == StudentID).ToList();

                List<StudentInformation> list = new List<StudentInformation>();
                foreach (var item in classStu)
                {
                    list.Add(student.GetEntity(item.StudentID));
                }


                return View(list);
            }
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