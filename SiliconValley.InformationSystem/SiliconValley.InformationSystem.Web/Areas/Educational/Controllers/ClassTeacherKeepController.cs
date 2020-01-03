using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.DepartmentBusiness;
using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.PositionBusiness;
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
        // GET: /Educational/ClassTeacherKeep/ClassTeacherTableData
        static Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();//获取登录人信息
        public ActionResult ClassTeacherKeepIndexView()
        {
            return View();
        }

        public ActionResult ClassTeacherTableData(int page,int limit)
        {
            EmployeesInfo_Entity = new EmployeesInfoManage();
            ClassTeacherKeep_Entity = new ClassTeacherKeepManeger();
            Position_Enity = new PositionManage();
            Headmaster_Entity = new HeadmasterBusiness();
            int find_postion = EmployeesInfo_Entity.GetEntity(UserName.EmpNumber).PositionId;//获取登录人的岗位
            int find_department = Position_Enity.GetEntity(find_postion).DeptId;//获取登录人的部门
            //获取所有班主任
            List<Headmaster> find_allheadmastet = Headmaster_Entity.GetList().Where(h => h.IsDelete == false).ToList();
            //获取这个部门的值班信息
            List<ClassTeacherKeep> ALL = new List<ClassTeacherKeep>();
            foreach (Headmaster item in find_allheadmastet)
            {
                EmployeesInfo find_e1 = EmployeesInfo_Entity.GetEntity(item.informatiees_Id);
                int find_depat1 = Position_Enity.GetEntity(find_e1.PositionId).DeptId;
                if (find_depat1 == find_department)
                {
                  ALL.AddRange(ClassTeacherKeep_Entity.GetAllClassTeacherKeep().Where(ct => ct.Headmaster_Id == item.informatiees_Id).ToList());
                }
            }
            
            var data = ALL.OrderByDescending(ct => ct.Id).Skip((page - 1) * limit).Take(limit).Select(ct=>new {
                Id=ct.Id,
                teachetName= EmployeesInfo_Entity.GetEntity( ct.Headmaster_Id).EmpName,
                ByDate =ct.ByDate,
                BySituation=ct.BySituation,//值班情况
                Rmark=ct.Rmark
            }).ToList();
            var jsondata = new { code=0,msg="",data=data,count=ALL.Count};
            return Json(jsondata,JsonRequestBehavior.AllowGet);
        }

        
         
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
            ClassTeacherKeep_Entity = new ClassTeacherKeepManeger();
            //获取值班类型
            int Zhitype =Convert.ToInt32( Request.Form[""]);
            //获取班主任
            string teacher = Request.Form[""];
            //日期
            DateTime time =Convert.ToDateTime( Request.Form[""]);
            //说明
            string ramke= Request.Form[""];
            ClassTeacherKeep new_t = new ClassTeacherKeep();
            new_t.BeOnDutyType_Id = Zhitype;
            new_t.ByDate = time;
            new_t.Headmaster_Id = teacher;
            new_t.IsDelete = false;
            new_t.Rmark = ramke;
            //判断是否有重复
            int count= ClassTeacherKeep_Entity.GetAllClassTeacherKeep().Where(ct => ct.ByDate == time && ct.Headmaster_Id == teacher && ct.IsDelete == false).ToList().Count;
            if (count<=0)
            {
                AjaxResult add_result = ClassTeacherKeep_Entity.Add_data(new_t);
                return Json(add_result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                AjaxResult result = new AjaxResult();
                result.Success = false;
                result.Msg = "该班主任已安排值班！！";
                return Json(result, JsonRequestBehavior.AllowGet);
            }            
        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditDataFunction()
        {
            ClassTeacherKeep_Entity = new ClassTeacherKeepManeger();
            int id=Convert.ToInt32( Request.Form[""]);//编号
            string emp = Request.Form[""];//班主任员工编号
            DateTime time =Convert.ToDateTime( Request.Form[""]);//安排日期
            string ramke = Request.Form[""];//修改原因
            int type =Convert.ToInt32( Request.Form[""]);//值班类型
            ClassTeacherKeep find_classteacherkeep= ClassTeacherKeep_Entity.GetEntity(id);
            find_classteacherkeep.BeOnDutyType_Id = type;
            find_classteacherkeep.ByDate = time;
            find_classteacherkeep.Headmaster_Id = emp;
            find_classteacherkeep.Rmark = ramke;
            AjaxResult edit_result=  ClassTeacherKeep_Entity.Edit_data(find_classteacherkeep);
            return Json(edit_result,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 手动添加或编辑页面
        /// </summary>
        /// <returns></returns>
        public ActionResult HandAddorEditView()
        {
            return View();
        }
    }
}