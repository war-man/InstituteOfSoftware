using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    [CheckLogin]
    public class SubstituteTeachCourseController : BaseMvcController
    {
        SubstituteTeachCourseManeger SubstituteTeachCourse_Entity;
        TeacherBusiness Teacher_Entity;
        EmployeesInfoManage EmployeesInfo_Entity;
        ClassScheduleBusiness ClassSchedule_Entity;
        ReconcileManeger Reconcile_Entity;
        // GET: /Educational/SubstituteTeachCourse/SubstituteTeachCourseIndexView
        public ActionResult SubstituteTeachCourseIndexView()
        {
            return View();
        }

        public ActionResult SubstituteTableData(int limit,int page)
        {
            SubstituteTeachCourse_Entity = new SubstituteTeachCourseManeger();
            Teacher_Entity = new TeacherBusiness();
            EmployeesInfo_Entity = new EmployeesInfoManage();
            ClassSchedule_Entity = new ClassScheduleBusiness();
            ClassSchedule_Entity = new ClassScheduleBusiness();
            List<SubstituteTeachCourse> s_list= SubstituteTeachCourse_Entity.GetList().OrderByDescending(s=>s.ApplyDate).ToList();
            var data = s_list.Skip((page - 1) * limit).Take(limit).Select(s => new
            {
                Id=s.Id,
                Applier= EmployeesInfo_Entity.GetEntity(Teacher_Entity.GetEntity(s.Applier).EmployeeId).EmpName,//申请人
                ApplyDate=s.ApplyDate,//申请时间
                Reson=s.Reson,//代课理由
                TeachDate=s.TeachDate,//代课日期,
                TeachDateSpec=s.TeachDateSpec,//上课时间段
                ClassNumber= ClassSchedule_Entity.GetEntity(s.ClassNumber).ClassNumber,//班级
                Teacher= EmployeesInfo_Entity.GetEntity(s.Teacher).EmpName,//代课老师
                isdel=s.IsDel
            }).ToList();

            var jsondata = new { code = 0, msg = "", count = s_list.Count, data = data };
            return Json(jsondata,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 审批代课
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ApprovalStubstitute(int id)
        {
            Reconcile_Entity = new ReconcileManeger();
            SubstituteTeachCourse_Entity = new SubstituteTeachCourseManeger();
            SubstituteTeachCourse find= SubstituteTeachCourse_Entity.GetEntity(id);
            AjaxResult a= Reconcile_Entity.Daike(find.TeachDate, find.Teacher, find.ClassNumber);//去排课表改数据
            if (a.Success==true)
            {
                //改为已审批
                find.IsDel = true;
               a.Success= SubstituteTeachCourse_Entity.Update_data(find);
            }           
            return Json(a,JsonRequestBehavior.AllowGet);
        }
    }
}