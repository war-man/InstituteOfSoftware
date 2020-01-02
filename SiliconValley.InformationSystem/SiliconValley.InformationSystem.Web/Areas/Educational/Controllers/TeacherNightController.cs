using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.DepartmentBusiness;
using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Business.PositionBusiness;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    [CheckLogin]
    public class TeacherNightController : Controller
    {
        // GET: /Educational/TeacherNight/HandAnpaiFunction

        TeacherNightManeger TeacherNight_Entity;
        TeacherBusiness Teacher_Entity;

        public ActionResult TeacherNightViewIndex()
        {
            return View();
        }

        static Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();//获取登录人信息
        static Recon_Login_Data GetBaseData(string Emp)
        {
            Recon_Login_Data new_re = new Recon_Login_Data();
            EmployeesInfo employees = Reconcile_Com.Employees_Entity.GetEntity(Emp);
            //获取部门
            DepartmentManage department = new DepartmentManage();
            Department find_d1 = department.GetList().Where(d => d.DeptName == "s1、s2教学部").FirstOrDefault();
            Department find_d2 = department.GetList().Where(d => d.DeptName == "s3教学部").FirstOrDefault();
            //获取岗位
            PositionManage position = new PositionManage();
            Position find_p = position.GetEntity(employees.PositionId);
            if (find_p.PositionName == "教务" && find_p.DeptId == find_d1.DeptId)
            {
                //s1.s2教务
                new_re.ClassRoom_Id = Reconcile_Com.ClassSchedule_Entity.BaseDataEnum_Entity.GetSingData("继善高科校区", false).Id;
                new_re.IsOld = true;
            }
            else
            {
                //s3教务
                new_re.ClassRoom_Id = Reconcile_Com.ClassSchedule_Entity.BaseDataEnum_Entity.GetSingData("达嘉维康校区", false).Id;
                new_re.IsOld = false;
            }
            return new_re;
        }
        //获取当前登录员是哪个校区的教务
        static Recon_Login_Data rr = GetBaseData(UserName.EmpNumber);
        static int base_id = rr.ClassRoom_Id;//确定教室 
        static bool IsOld = rr.IsOld;//确定教务
        public ActionResult TeacherTableData(int page,int limit)
        {
            TeacherNight_Entity = new TeacherNightManeger();
            List<TeacherNight> getall= TeacherNight_Entity.GetAllTeacherNight();
            var data = getall.OrderByDescending(t => t.Id).Skip((page - 1) * limit).Take(limit).Select(t => new
            {
                Id=t.Id,
                IsDelete=t.IsDelete,
                OrwatchDate=t.OrwatchDate,
                Rmark=t.Rmark,
                TearcherName=Reconcile_Com.Employees_Entity.GetEntity( t.Tearcher_Id).EmpName,
                timename=t.timename,
                ClassroomName=Reconcile_Com.Classroom_Entity.GetEntity( t.ClassRoom_id).ClassroomName,
                ClassNumber=Reconcile_Com.ClassSchedule_Entity.GetEntity( t.ClassSchedule_Id).ClassNumber
            }).ToList();
            var jsondata = new { count=getall.Count,code=0,msg="",data=data};
            return Json(jsondata,JsonRequestBehavior.AllowGet);
        }

        public ActionResult AnpaiTeacherNight()
        {
            return null;
        }

        public ActionResult AddDataView()
        {             
            //获取所有老师
            Teacher_Entity = new TeacherBusiness();
            List<SelectListItem> sle_teacher= Teacher_Entity.GetTeachers().Select(t=>new SelectListItem() { Text=Reconcile_Com.Employees_Entity.GetEntity(t.EmployeeId).EmpName,Value=t.EmployeeId}).ToList();
            sle_teacher.Add(new SelectListItem() { Text="--请选择--",Value="0",Selected=true});
            ViewBag.teacher = sle_teacher;
            //获取教室
            List<SelectListItem> sle_classroom = Reconcile_Com.Classroom_Entity.GetList().Where(c => c.BaseData_Id == base_id && c.IsDelete == false).Select(c => new SelectListItem()
            {
                Text = c.ClassroomName,
                Value= c.Id.ToString()
            }).ToList();
            sle_classroom.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.classroom = sle_classroom;
            //获取班级
           List<SelectListItem> sle_class= Reconcile_Com.GetClass(IsOld).Select(cl => new SelectListItem() { Text = cl.ClassNumber, Value = cl.id.ToString() }).ToList();
            sle_class.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.myclass = sle_class;
            return View();
        }
        [HttpPost]
        public ActionResult SystemAnpaiFunction()
        {
            TeacherNight_Entity = new TeacherNightManeger();
            string[] times= Request.Form["times"].Split('到');
            DateTime start = Convert.ToDateTime(times[0]);
            DateTime end = Convert.ToDateTime(times[1]);
            AjaxResult a= TeacherNight_Entity.AnpaiNight(start, end, IsOld);
            return Json(a,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult HandAnpaiFunction()
        {
            AjaxResult a = new AjaxResult();
            TeacherNight_Entity = new TeacherNightManeger();
            string timename= Request.Form["timename"];
            DateTime date = Convert.ToDateTime(Request.Form["mytime"]);
            int class_id =Convert.ToInt32( Request.Form["classShdule_sele"]);
            string teacherEmp = Request.Form["teacher_sele"];
            int classroom_id =Convert.ToInt32( Request.Form["classroom_sele"]);
            string ramke = Request.Form["ramke"];
            TeacherNight new_t = new TeacherNight();
            new_t.OrwatchDate = date;
            new_t.ClassRoom_id = classroom_id;
            new_t.ClassSchedule_Id = class_id;
            new_t.IsDelete = false;
            new_t.Tearcher_Id = teacherEmp;
            new_t.Rmark = ramke;
            //判断是否有重复的数据
            int count= TeacherNight_Entity.GetAllTeacherNight().Where(tea => tea.ClassSchedule_Id == new_t.ClassSchedule_Id && tea.OrwatchDate == new_t.OrwatchDate).ToList().Count;
            if (count>0)
            {
                a.Success = false;
                a.Msg = "该班级已安排值班老师";
            }
            else
            {
               a= TeacherNight_Entity.Add_data(new_t);
            }
            return Json(a,JsonRequestBehavior.AllowGet); ;
        }
    }
}