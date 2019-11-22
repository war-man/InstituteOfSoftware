﻿using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    public class HeadteacherController : Controller
    {
        public static int HeadteID;
        // GET: Teachingquality/Headteacher
        private readonly HeadmasterBusiness dbtext;
        //员工信息
        EmployeesInfoManage employeesInfoManage = new EmployeesInfoManage();
        //员工岗位
        BaseBusiness<Position> business = new BaseBusiness<Position>();
        //人事,部门表
        BaseBusiness<Department> Depa = new BaseBusiness<Department>();
        //班级
        ClassScheduleBusiness ClasHead = new ClassScheduleBusiness();
        //班主任带班
        BaseBusiness<HeadClass> HeadClassEnti = new BaseBusiness<HeadClass>();
        public HeadteacherController()
        {
            dbtext = new HeadmasterBusiness();
        }
        //主页面
        public ActionResult Index()
        {
            return View();
        }
        //班主任数据
        public ActionResult GetDate(int page, int limit)
        {

          //  int DepaID = Depa.GetList().Where(a => a.DeptName.Contains("教质部") && a.IsDel == false).FirstOrDefault().DeptId;
            var list = dbtext.GetList().Where(a=>a.IsDelete==false).ToList();
            //    List<EmployeesInfo> EmployeesInfoList = new List<EmployeesInfo>();


            var emp=   employeesInfoManage.GetList().Where(a => a.IsDel == false).ToList();
       var dataList = list.Select(c=>new { c.informatiees_Id, informatiees_Name = employeesInfoManage.GetEntity(c.informatiees_Id).EmpName,
        informatiees_Sex = emp.Where(a=>a.EmployeeId==c.informatiees_Id).FirstOrDefault().Sex ,
        Name = business.GetList().Where(a=>a.Pid== emp.Where(q=>q.EmployeeId==c.informatiees_Id).FirstOrDefault().PositionId&&a.IsDel==false).FirstOrDefault().PositionName,
        EntryTime = emp.Where(a=>a.EmployeeId==c.informatiees_Id).FirstOrDefault().EntryTime,
        ID=c.ID
    }).OrderBy(a => a.informatiees_Id).Skip((page - 1) * limit).Take(limit).ToList();
        //  var x = dbtext.GetList();
        var data = new
        {
            code = "",
            msg = "",
            count = list.Count,
            data = dataList
        };
            return Json(data, JsonRequestBehavior.AllowGet);
    }
        //接班页面
        public ActionResult HeadmasterEntity(int ID)
        {
            HeadteID = ID;

            ViewBag.HeadMa = HeadmasreClass();
            ViewBag.Name = employeesInfoManage.GetEntity(dbtext.GetEntity(ID).informatiees_Id).EmpName;
           
            return View();
        }

        //班主任职业素养培训
        BaseBusiness<Professionala> ProfessionalaBusiness = new BaseBusiness<Professionala>();
        //班级数据

        public string HeadmasreClass()
        {
           
            //带班人
            object obj = new object();
            List<ClassSchedule> ListClass = ClasHead.GetList().Where(a=>a.ClassstatusID==null).ToList();
            List<ClassSchedule> MyClass = new List<ClassSchedule>();
           var x=   HeadClassEnti.GetList().Where(a => a.IsDelete == false && a.LeaderID == HeadteID).ToList();
            //3
            var x1 = HeadClassEnti.GetList().Where(a => a.IsDelete == false && a.LeaderID != HeadteID).ToList();
            //0
            foreach (var item in x)
            {
                var classstudent = ClasHead.GetList().Where(a => a.IsDelete == false && a.ClassStatus == false && a.id == item.ClassID&& a.ClassstatusID == null).FirstOrDefault();
                if (classstudent!=null)
                {
                    MyClass.Add(classstudent);
                }
               
            }
            foreach (var item in x1)
            {
                ListClass = ListClass.Where(a => a.IsDelete == false && a.ClassStatus == false && a.id != item.ClassID).ToList();
            }

            var data=new
            {
                d1=ListClass.Select(c => new { value = c.ClassNumber, title = c.ClassNumber }).ToList(),
                d2= MyClass.Select(c => new { c.ClassNumber}).ToList()
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(data);
            
        }
        public ActionResult HeadmasreEntity()
        {
            string ClassName = Request.QueryString["ClassName"];

            return Json(dbtext.HeadClassEntis(HeadteID.ToString(), ClassName), JsonRequestBehavior.AllowGet); ;
        }


        //登记班主任职业素养课件培训
        [HttpGet]
        public ActionResult AddProfessionala()
        {
            return View();
        }
        //班主任职业素养培训数据记录
        [HttpPost]
        public ActionResult AddProfessionala(Professionala professionala)
        {
            return Json(dbtext.AddProfessionala(professionala),JsonRequestBehavior.AllowGet);
        }
        //显示班主任职业素养课件培训
        public ActionResult GetProfessionala()
        {
            ViewBag.professionala = ProfessionalaBusiness.GetList().Where(a => a.Dateofregistration == false).ToList().Count;
            return View();
        }
        /// <summary>
        /// 班主任职业素养培训数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetDateProfessionala(int page, int limit)
        {
            return Json(dbtext.GetDateProfessionala(page, limit), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询单条班主任职业素养培训课件
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ActionResult FineProfessionala(int id)
        {
            return View(dbtext.FineProfessionala(id));
        }
        /// <summary>
        /// 班主任带班分布图
        /// </summary>
        /// <returns></returns>
        public ActionResult Teamleaderdistribution()
        {
           ViewBag.ListTeam= Newtonsoft.Json.JsonConvert.SerializeObject(dbtext.ListTeamleaderdistributionView());
            ViewBag.ListTeamleaderdistributionView = dbtext.ListTeamleaderdistributionView();
            return View();
        }
        public string Text()
        {
          
            return null;
        }
    }
}