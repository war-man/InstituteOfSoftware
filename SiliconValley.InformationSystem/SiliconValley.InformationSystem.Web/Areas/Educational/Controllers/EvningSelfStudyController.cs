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
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Business.Base_SysManage;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    [CheckLogin]
    public class EvningSelfStudyController : Controller
    {
        // GET: /Educational/EvningSelfStudy/EvningSelfStudyIndexView

        EvningSelfStudyManeger EvningSelefstudy_Entity;
       

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
        static Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();//获取登录人信息
        //获取当前登录员是哪个校区的教务
        static Recon_Login_Data rr = GetBaseData(UserName.EmpNumber);
        static int base_id = rr.ClassRoom_Id;//确定校区
        static bool IsOld = rr.IsOld;//确定教务

        public ActionResult EvningSelfStudyIndexView()
        {
            //获取阶段
            List<SelectListItem> g_list = Reconcile_Com.GetGrand_Id(IsOld).Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.grlist = g_list;
            return View();
        }

        public ActionResult EvningTableData(int page,int limit)
        {
            EvningSelefstudy_Entity = new EvningSelfStudyManeger();
            List<EvningSelfStudy> selfStudies = EvningSelefstudy_Entity.EvningSelfStudyGetAll();
            List<Grand> glist= Reconcile_Com.GetGrand_Id(IsOld);
            List<EvningSelfStudy> Evn_list = new List<EvningSelfStudy>();
            foreach (EvningSelfStudy item in selfStudies)
            {
                int grand_id =Reconcile_Com.ClassSchedule_Entity.GetEntity(item.ClassSchedule_id).grade_Id;
                int count = glist.Where(g => g.Id == grand_id).ToList().Count;
                if (count>0)
                {
                    Evn_list.Add(item);
                }
            }
             string c_id=  Request.QueryString["class_selectone"];
            string startime = Request.QueryString["onetime"];
            string endtime = Request.QueryString["twotime"];
            if (!string.IsNullOrEmpty(c_id))
            {
                int class_id =int.Parse( c_id);
                Evn_list = Evn_list.Where(e => e.ClassSchedule_id == class_id).ToList();
            }
            if (!string.IsNullOrEmpty(startime))
            {
               DateTime d1= Convert.ToDateTime(startime);
                Evn_list = Evn_list.Where(e => e.Anpaidate>=d1).ToList();
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                DateTime d2 = Convert.ToDateTime(endtime);
                Evn_list = Evn_list.Where(e => e.Anpaidate<=d2).ToList();
            }
            var data = Evn_list.Skip((page - 1) * limit).Take(limit).OrderBy(e => e.id).Select(e=>new {
                Id=e.id,
                EmpName=string.IsNullOrEmpty(e.emp_id)==true?null:Reconcile_Com.Employees_Entity.GetEntity(e.emp_id).EmpName,
                Anpaidate=e.Anpaidate,
                ClassroomName=Reconcile_Com.Classroom_Entity.GetEntity( e.Classroom_id).ClassroomName,
                ClassSchedule_id=Reconcile_Com.ClassSchedule_Entity.GetEntity( e.ClassSchedule_id).ClassNumber,
                curd_name=e.curd_name,
                Rmark = e.Rmark
            });

            var jsonData = new { code=0,msg="",data=data,count= Evn_list .Count};
            return Json(jsonData,JsonRequestBehavior.AllowGet); 
        }

        public ActionResult UpdateEvning(int id)
        {
            EvningSelefstudy_Entity = new EvningSelfStudyManeger();
            //获取班级
             List<SelectListItem> class_select = Reconcile_Com.GetClass(true).Select(e => new SelectListItem() { Text = e.ClassNumber, Value = e.id.ToString() }).ToList();
            ViewBag.Classlist = class_select;
            //获取教室
            List<SelectListItem> classrooms = Reconcile_Com.Classroom_Entity.GetAddreeClassRoom(base_id).Select(c=>new SelectListItem() { Text=c.ClassroomName,Value=c.Id.ToString()}).ToList();
            ViewBag.classroom = classrooms;
            //获取上课时间段
            List<SelectListItem> timename_list = new List<SelectListItem>();
            timename_list.Add(new SelectListItem() { Text="晚一" ,Value="晚一"});
            timename_list.Add(new SelectListItem() { Text = "晚二", Value = "晚二" });
            ViewBag.timename = timename_list;
            EvningSelfStudy find_e= EvningSelefstudy_Entity.GetEntity(id);
            //获取老师
            ViewBag.teacher = string.IsNullOrEmpty(find_e.emp_id) == true ? null : Reconcile_Com.Employees_Entity.GetEntity(find_e.emp_id).EmpName;

            //获取排课日期
            ViewBag.date = find_e.Anpaidate.Year + "-" + find_e.Anpaidate.Month + "-" + find_e.Anpaidate.Day;
            return View(find_e);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateFunction(EvningSelfStudy e)
        {
            EvningSelefstudy_Entity = new EvningSelfStudyManeger();
            AjaxResult a= EvningSelefstudy_Entity.Update_DataTwo(e);
            return Json(a,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            EvningSelefstudy_Entity = new EvningSelfStudyManeger();
            AjaxResult a = EvningSelefstudy_Entity.Delete_Data(id);
            return Json(a,JsonRequestBehavior.AllowGet);
        }

        //全体调课
        public ActionResult BigDataADIview()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BigDataADIfunction()
        {
            DateTime startime=Convert.ToDateTime( Request.Form["oldTime"]);
            DateTime endtime = Convert.ToDateTime(Request.Form["endtime"]);
            var days= endtime.Subtract(startime);
            int count = days.Days;
            EvningSelefstudy_Entity = new EvningSelfStudyManeger();
            AjaxResult a= EvningSelefstudy_Entity.ALLDataADI(IsOld, count, startime);
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        
        //班级调课
        public ActionResult ClassDataADIview()
        {
            //获取阶段
            List<SelectListItem> g_list= Reconcile_Com.GetGrand_Id(IsOld).Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text="--请选择--",Value="0"});
            ViewBag.grlist = g_list;
            return View();
        }

        [HttpPost]
        public ActionResult ClassDataADIfunction()
        {
            DateTime startime=Convert.ToDateTime( Request.Form["starTime"]);
            DateTime endtime = Convert.ToDateTime( Request.Form["endTime"]);
            var days= endtime.Subtract(startime);
            int count = days.Days;
            int class_id = Convert.ToInt32(Request.Form["class_select"]);
            EvningSelefstudy_Entity = new EvningSelfStudyManeger();
            AjaxResult a= EvningSelefstudy_Entity.ClassALLDataADI(count, startime, class_id);
            return Json(a,JsonRequestBehavior.AllowGet);
        }

        //全体上课日期调换
        public ActionResult BigDataChangView()
        {
            return View();
        }
        [HttpPost]
        public ActionResult BigDataChangFunction()
        {
            DateTime startime = Convert.ToDateTime(Request.Form["oldTime"]);
            DateTime endtime = Convert.ToDateTime(Request.Form["endtime"]);

            EvningSelefstudy_Entity = new EvningSelfStudyManeger();
            List<EvningSelfStudy> find_e=  EvningSelefstudy_Entity.EvningSelfStudyGetAll().Where(e => e.Anpaidate == startime).ToList();
            AjaxResult a= EvningSelefstudy_Entity.ChangDate(find_e, endtime);
            return Json(a,JsonRequestBehavior.AllowGet);
        }

        //班级上课日期调换
        public ActionResult BigDataClassChangView()
        {
            //获取阶段
            List<SelectListItem> g_list = Reconcile_Com.GetGrand_Id(IsOld).Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0" });
            ViewBag.grlist = g_list;
            return View();
        }

        [HttpPost]
        public ActionResult ClassDataClassChangfunction()
        {
            EvningSelefstudy_Entity = new EvningSelfStudyManeger();
            DateTime startime = Convert.ToDateTime(Request.Form["starTime"]);
            DateTime endtime = Convert.ToDateTime(Request.Form["endTime"]);  
            int class_id = Convert.ToInt32(Request.Form["class_select"]);
            List<EvningSelfStudy> find_e= EvningSelefstudy_Entity.EvningSelfStudyGetAll().Where(e => e.ClassSchedule_id == class_id && e.Anpaidate == startime).ToList();
            AjaxResult a = EvningSelefstudy_Entity.ChangDate(find_e, endtime);
            return Json(a, JsonRequestBehavior.AllowGet);
        }

    }
}