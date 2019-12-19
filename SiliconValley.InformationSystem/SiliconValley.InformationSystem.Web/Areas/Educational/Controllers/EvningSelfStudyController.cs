using SiliconValley.InformationSystem.Business.DepartmentBusiness;
using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Business.PositionBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    public class EvningSelfStudyController : Controller
    {
        // GET: Educational/EvningSelfStudy

        EvningSelfStudyManeger EvningSelefstudy_Entity;
        public ActionResult EvningSelfStudyIndexView()
        {
            return View();
        }
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
        static Recon_Login_Data rr = GetBaseData("201911190041");
        static int base_id = rr.ClassRoom_Id;//确定校区
        static bool IsOld = rr.IsOld;//确定教务

        public ActionResult EvningTableData(int page,int limit)
        {
            EvningSelefstudy_Entity = new EvningSelfStudyManeger();
            List<EvningSelfStudy> selfStudies = EvningSelefstudy_Entity.EvningSelfStudyGetAll();
            List<Grand> glist= Reconcile_Com.GetGrand_Id(IsOld);
            return null;
        }
    }
}