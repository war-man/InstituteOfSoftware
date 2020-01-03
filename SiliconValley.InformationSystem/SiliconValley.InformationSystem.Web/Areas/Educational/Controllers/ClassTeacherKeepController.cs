using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.DepartmentBusiness;
using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.PositionBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    /// <summary>
    /// 班主任值班记录
    /// </summary>
    [CheckLogin]
    public class ClassTeacherKeepController : Controller
    {
        ClassTeacherKeepManeger ClassTeacherKeep_Entity;
        PositionManage Position_Enity;
        EmployeesInfoManage EmployeesInfo_Entity;
        ReconcileManeger Reconcile_Entity;
        HeadmasterBusiness Headmaster_Entity;
        BeOnDutyManeger BeOnDuty_Entity;
        // GET: /Educational/ClassTeacherKeep/ClassTeacherKeepIndexView

        public ActionResult ClassTeacherKeepIndexView()
        {
            return View();
        }

        public ActionResult ClassTeacherTableData(int page,int limit)
        {
            ClassTeacherKeep_Entity = new ClassTeacherKeepManeger();
            List<ClassTeacherKeep> ALL=ClassTeacherKeep_Entity.GetAllClassTeacherKeep();
            var data = ALL.OrderByDescending(ct => ct.Id).Skip((page - 1) * limit).Take(limit).Select(ct=>new {
                Id=ct.Id,
                teachetName= ct.Headmaster_Id,
                ByDate=ct.ByDate,
                BySituation=ct.BySituation,//值班情况
                Rmark=ct.Rmark
            }).ToList();
            var jsondata = new { code=0,msg="",data=data,count=ALL.Count};
            return Json(jsondata,JsonRequestBehavior.AllowGet);
        }

        static Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();//获取登录人信息
         
        /// <summary>
        /// 系统安排班主任晚自习值班
        /// </summary>
        /// <returns></returns>
        public ActionResult SysAnpaiFunction()
        {
            //判断登录人是哪个部门的
            EmployeesInfo_Entity = new EmployeesInfoManage();
            Position_Enity = new PositionManage();
            Reconcile_Entity = new ReconcileManeger();
            Headmaster_Entity = new HeadmasterBusiness();
            BeOnDuty_Entity = new BeOnDutyManeger();
            ClassTeacherKeep_Entity = new ClassTeacherKeepManeger();
            int find_postion= EmployeesInfo_Entity.GetEntity(UserName.EmpNumber).PositionId;//获取登录人的岗位
            int find_department = Position_Enity.GetEntity(find_postion).DeptId;//获取登录人的部门
            //获取所有班主任
            List<Headmaster> find_allheadmastet=  Headmaster_Entity.GetList().Where(h => h.IsDelete == false).ToList();
             //获取
             List<EmployeesInfo> list = new List<EmployeesInfo>();
            foreach (Headmaster item in find_allheadmastet)
            {
                EmployeesInfo find_e1 = EmployeesInfo_Entity.GetEntity(item.informatiees_Id);
                int find_depat1= Position_Enity.GetEntity(find_e1.PositionId).DeptId;
                if (find_depat1== find_department)
                {
                    list.Add(find_e1);
                }
            }
            //获取安排的月份
            string[] dd = Request.Form["time"].Split('到') ;
            DateTime d1 =Convert.ToDateTime(dd[0]);
            DateTime d2 = Convert.ToDateTime(dd[1]);
            int index = 0;
            List<ClassTeacherKeep> new_list = new List<ClassTeacherKeep>();
            while (d1<=d2)
            {
                //判断这天是否可以安排晚自习值班
                bool s1= Reconcile_Entity.IsShangKe(Server.MapPath("~/Xmlconfigure/Reconcile_XML.xml"), d1);
                if (s1)
                {
                    ClassTeacherKeep new_cT = new ClassTeacherKeep();
                    BeOnDuty find_beonduty= BeOnDuty_Entity.GetSingleBeOnButy("晚自习", false);
                    new_cT.ByDate = d1;
                    if (index==list.Count)
                    {
                        index = 0;
                    }
                    new_cT.Headmaster_Id = list[index].EmployeeId;
                    new_cT.IsDelete = false;
                    new_cT.BeOnDutyType_Id = find_beonduty != null ? find_beonduty.Id : 1;
                    new_list.Add(new_cT);
                    index++;
                }
                else
                {
                    d1 = d1.AddDays(1);
                }
            }
            bool State = ClassTeacherKeep_Entity.LIST_add(new_list);
            return Json(State,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 手动安排值班
        /// </summary>
        /// <returns></returns>
        public ActionResult HandAnpaiFunction()
        {
            //获取值班类型
            string Zhitype = Request.Form[""];
            //获取班主任
            string teacher = Request.Form[""];
            //日期
            DateTime time =Convert.ToDateTime( Request.Form[""]);

            return null;
        }
    }
}