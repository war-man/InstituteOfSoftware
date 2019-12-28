using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    public class LeaderAddCourseToExamineController : Controller
    {
        private AddCourseManeger AddCourse_Entity;
        private EmployeesInfoManage Employees_Entity;
        private ClassScheduleBusiness ClassSchedule_Entity;
        private  CourseBusiness Curriculum_Entity ;
        private  TeacherBusiness Teacher_Entity;
        private ReconcileManeger Reconcile_Entity;
        // GET: /Educational/LeaderAddCourseToExamine/LeaderAddCourseToExamineIndex
        public ActionResult LeaderAddCourseToExamineIndex()
        {
            return View();
        }

        public ActionResult GetLeaderAddCourseTableData(int limit,int page)
        {
            AddCourse_Entity = new AddCourseManeger();
            Employees_Entity = new EmployeesInfoManage();
            ClassSchedule_Entity = new ClassScheduleBusiness();
            Curriculum_Entity = new CourseBusiness();
            Teacher_Entity = new TeacherBusiness();
            List<AddCourse> AddCourse_list= AddCourse_Entity.GetALLAddCourseData();
            var data = AddCourse_list.OrderByDescending(a => a.ApplyDate).Skip((page - 1) * limit).Take(limit).Select(a => new
            {
                Id=a.ID,
                Isdel=a.Isdel,
                reason=a.reason,
                SpecDate=a.SpecDate,
                TeachDate=a.TeachDate,
                TeacherIDname= Employees_Entity.GetEntity( Teacher_Entity.GetTeacherByID(a.TeacherID).EmployeeId).EmpName,
                ApplyDate=a.ApplyDate,
                ClassNumber= ClassSchedule_Entity.GetEntity(a.ClassNumber).ClassNumber,
                Count= a.Count,
                Course= Curriculum_Entity.GetEntity(a.Course),               
            }).ToList();

            var jsondata = new { count= AddCourse_list .Count,code=0,msg="",data=data};

            return Json(jsondata,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 批准加课
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GreeAddCourse(int id)
        {
            Reconcile_Entity = new ReconcileManeger();
            Teacher_Entity = new TeacherBusiness();
            AddCourse_Entity = new AddCourseManeger();
            Curriculum_Entity = new CourseBusiness();
             AddCourse find= AddCourse_Entity.GetEntity(id);
            AjaxResult a= Reconcile_Entity.Addke(find.TeachDate, Teacher_Entity.GetEntity(find.TeacherID).EmployeeId, find.SpecDate, find.ClassNumber, Curriculum_Entity.GetEntity(find.Course).CourseName, find.Count, Server.MapPath("~/Xmlconfigure/Reconcile_XML.xml"));
            if (a.Success==true)
            {
                find.Isdel = true;
                AddCourse_Entity.Update(find);
            }
            return Json(a,JsonRequestBehavior.AllowGet);
        }
    }
}