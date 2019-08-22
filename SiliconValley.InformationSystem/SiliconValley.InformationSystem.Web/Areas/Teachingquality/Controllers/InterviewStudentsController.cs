using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Business.StudentmanagementBusinsess;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    public class InterviewStudentsController : Controller
    {
        // GET: Teachingquality/InterviewStudents
        //学员班级表
        ScheduleForTraineesBusiness Stuclass = new ScheduleForTraineesBusiness();
        //学员表
        StudentInformationBusiness student = new StudentInformationBusiness();
        //员工表
        EmployeesInfoManage infoBusiness = new EmployeesInfoManage();
        //班级表
        ClassScheduleBusiness classschedu = new ClassScheduleBusiness();
        //实例化
        private readonly InterviewStudentsBusiness dbtext;
        public InterviewStudentsController()
        {
            dbtext = new InterviewStudentsBusiness();
        }
        //主页面
        public ActionResult Index()
        {
            ViewBag.List = classschedu.GetList().Where(a => a.IsDelete == false && a.ClassStatus == false).Select(a => new SelectListItem { Text = a.ClassNumber, Value = a.ClassNumber });

            return View();
        }
        //主页面获取表格数据
        public ActionResult GetDate(int page, int limit)
        {
            var list = dbtext.Mylist("name");
            var mylist = list.Select(a => new
            {
                ID=a.ID,
                InterviewTopics = a.InterviewTopics, //谈话标题
                Interviewcontent = a.Interviewcontent, //谈话内容
                StudentNumberID = a.StudentNumberID,//谈话学员学号
                StudentNumberName = student.GetEntity(a.StudentNumberID).Name,//谈话学员姓名
                StuClass = Stuclass.GetList().Where(c => c.StudentID == a.StudentNumberID && c.CurrentClass == true).FirstOrDefault().ClassID, //谈话学员班级
                InterviewRecorderID = a.InterviewRecorderID, //记录人编号
                InterviewRecorderName = infoBusiness.GetEntity(a.InterviewRecorderID).EmpName, //姓名
                Dateofinterview = a.Dateofinterview //谈话日期
            });
            var dataList = mylist.OrderBy(a => a.ID).Skip((page - 1) * limit).Take(limit).ToList();
          
            var data = new
            {
                code = "",
                msg = "",
                count = dataList.Count,
                data = dataList
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        //添加 编辑学员谈话记录
        public ActionResult StudIenterEntiy()
        {
            string id = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(id) && id != "undefined")
            {

                ViewBag.Name = "修改学员访谈记录";
                ViewBag.id = id;
                return View();

            }
            ViewBag.id = "";
            ViewBag.Name = "登记学员访谈记录";
            return View();
        }

        //查询数据库是否由重复数据
        public ActionResult Errnt(InterviewStudents interviewStudents)
        {

            string tu = interviewStudents.StudentNumberID.Substring(0, interviewStudents.StudentNumberID.Length);
            //转成数组
            string[] stu = tu.Split(',');
            var list = dbtext.Mylist("name");
            List<InterviewStudents> mylist = new List<InterviewStudents>();
            for (int i = 0; i < stu.Length; i++)
            {

                var count = list.Where(a => a.StudentNumberID == stu[i] && a.InterviewTopics == interviewStudents.InterviewTopics && a.Dateofinterview == interviewStudents.Dateofinterview).ToList();
                if (count.Count > 0)
                {
                    mylist.AddRange(count);
                }
            }

            return Json(mylist.Select(a => new { Name = student.GetEntity(a.StudentNumberID).Name, StudentID = a.StudentNumberID, Dateofinterview=a.Dateofinterview }), JsonRequestBehavior.AllowGet);


        }

        //数据提交到数据库
        [HttpPost]
        public ActionResult StudIenterEntiy(InterviewStudents interviewStudents)
        {
            string EmpNumber = "201908150001";
            AjaxResult result = null;
            //多个学号组成后面截取逗号
            if (interviewStudents.ID>0)
            {
                try
                {
                   
                    var x = dbtext.GetEntity(interviewStudents.ID);
                    interviewStudents.InterviewRecorderID = x.InterviewRecorderID;
                    interviewStudents.Dateofinterview = x.Dateofinterview;
                    interviewStudents.IsDelete = x.IsDelete;
                    interviewStudents.StudentNumberID = x.StudentNumberID;
                    dbtext.Update(interviewStudents);
                    result = new SuccessResult();
                    result.Msg = "修改谈话记录成功";
                    result.Success = true;
                    dbtext.Remove("name");
                }
                catch (Exception ex)
                {


                    result = new ErrorResult();
                    result.ErrorCode = 500;
                    result.Success = false;
                    result.Msg = "服务器错误";
                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据异常);
                }
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else
            {
                string tu = interviewStudents.StudentNumberID.Substring(0, interviewStudents.StudentNumberID.Length);
                //转成数组
                // Session["EmpNumber"].ToString();
            
                string[] stu = tu.Split(',');
              
                try
                {
                    List<InterviewStudents> list = new List<InterviewStudents>();
                    for (int i = 0; i < stu.Length; i++)
                    {
                        InterviewStudents Students = new InterviewStudents();
                        Students.InterviewTopics = interviewStudents.InterviewTopics;
                        Students.Dateofinterview = interviewStudents.Dateofinterview;
                        Students.Interviewcontent = interviewStudents.Interviewcontent;
                        Students.InterviewRecorderID = EmpNumber;
                        Students.StudentNumberID = stu[i];
                        Students.Addtime = DateTime.Now;
                        Students.IsDelete = false;

                        list.Add(Students);
                    }
                    dbtext.BulkInsert(list);
                    result = new SuccessResult();
                    result.Msg = "记录成功";
                    result.Success = true;
                }
                catch (Exception ex)
                {

                    result = new ErrorResult();
                    result.ErrorCode = 500;
                    result.Success = false;
                    result.Msg = "服务器错误";
                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据异常);
                    dbtext.Remove("name");
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
           
           
        }

        //编辑页面赋值数据
        public ActionResult Select(int id)
        {
            try
            {
                var x = dbtext.GetList().Where(a => a.ID == id && a.IsDelete == false).Select(c => new
                {
                    ID = c.ID,
                    InterviewTopics = c.InterviewTopics,
                    Interviewcontent = c.Interviewcontent,
                    Dateofinterview = c.Dateofinterview,
                    StudentNumberID = student.GetList().Where(w => w.StudentNumber == c.StudentNumberID && w.IsDelete != true).FirstOrDefault().Name


                });
                return Json(x, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据异常);
                return Json("数据异常", JsonRequestBehavior.AllowGet);
            }
        
        }


    }
}