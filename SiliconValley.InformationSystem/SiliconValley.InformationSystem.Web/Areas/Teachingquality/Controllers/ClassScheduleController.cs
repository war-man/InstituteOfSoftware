﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.StageGrade_Business;
using SiliconValley.InformationSystem.Business.BaseDataEnum_Business;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.Common;
//班级管理
namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    
    public class ClassScheduleController : Controller
    {
        private static string classNumberss = "";
        private readonly ClassScheduleBusiness dbtext;
        public ClassScheduleController()
        {
            dbtext = new ClassScheduleBusiness();

        }
        //学员班级表
        ScheduleForTraineesBusiness Stuclass = new ScheduleForTraineesBusiness();
        // GET: Teachingquality/ClassSchedule
        //主页面
        public ActionResult Index()
        {
            //专业课时段
            ViewBag.BaseDataEnum_Id = BanseDatea.GetList().Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() });
            //专业
            ViewBag.Major_Id = Techarcontext.GetList().Select(a => new SelectListItem { Text = a.SpecialtyName, Value = a.Id.ToString() });
            //阶段
            ViewBag.grade_Id = Grandcontext.GetList().Select(a => new SelectListItem { Text = a.GrandName, Value = a.Id.ToString() });
            return View();
        }
        //基础数据枚举数据
        BaseDataEnumBusiness BanseDatea = new BaseDataEnumBusiness();
        //专业
        SpecialtyBusiness Techarcontext = new SpecialtyBusiness();
        //阶段
        GrandBusiness Grandcontext = new GrandBusiness();
        //获取数据
        public ActionResult GetDate(int page ,int limit,string ClassNumber,string Major_Id,string grade_Id,string BaseDataEnum_Id)
        {

         
           
            try
            {
         List<ClassSchedule> list = dbtext.GetList().Where(a=>a.ClassStatus==false&&a.IsDelete==false).ToList();
            if (!string.IsNullOrEmpty(ClassNumber))
            {
                list = list.Where(a => a.ClassNumber.Contains(ClassNumber)).ToList();
            }
            if (!string.IsNullOrEmpty(Major_Id))
            {
                int maid = int.Parse(Major_Id);
                list = list.Where(a => a.Major_Id== maid).ToList();
            }
            if (!string.IsNullOrEmpty(grade_Id))
            {
                int maid = int.Parse(grade_Id);
                list = list.Where(a => a.grade_Id == maid).ToList();
            }
            if (!string.IsNullOrEmpty(BaseDataEnum_Id))
            {
                int maid = int.Parse(BaseDataEnum_Id);
                list = list.Where(a => a.BaseDataEnum_Id == maid).ToList();
            }

            var listx = list.Select(a => new
            {
                //  a.BaseDataEnum_Id,
                ClassNumber = a.ClassNumber,
                ClassRemarks = a.ClassRemarks,
                ClassStatus = a.ClassStatus,
                IsDelete = a.IsDelete,
                grade_Id = Grandcontext.GetEntity(a.grade_Id).GrandName, //阶段id
                BaseDataEnum_Id = BanseDatea.GetEntity(a.BaseDataEnum_Id).Name,//专业课时间
                Major_Id = Techarcontext.GetEntity(a.Major_Id).SpecialtyName,//专业
                 stuclasss = Stuclass.GetList().Where(c=>c.ClassID==a.ClassNumber&&c.CurrentClass==true).Count()//专业
            }).ToList();
            var dataList = listx.OrderBy(a => a.ClassNumber).Skip((page - 1) * limit).Take(limit).ToList();
            //  var x = dbtext.GetList();
            var data = new
            {
                code = "",
                msg = "",
                count = dataList.Count,
                data = dataList
            }; return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.加载数据);
                return Json("数据有误", JsonRequestBehavior.AllowGet);
            }
        }

         //开设班级页面
         [HttpGet]
        public ActionResult AddClassSchedule()
        {
          
            //专业课时段
            ViewBag.BaseDataEnum_Id = BanseDatea.GetList().Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() });
            //专业
            ViewBag.Major_Id = Techarcontext.GetList().Select(a => new SelectListItem { Text = a.SpecialtyName, Value = a.Id.ToString() });
                //阶段
                ViewBag.grade_Id = Grandcontext.GetList().Select(a => new SelectListItem { Text = a.GrandName, Value = a.Id.ToString() });
            return View();
        }
        //添加班级数据
        [HttpPost]
        public ActionResult AddClassSchedule(ClassSchedule classSchedule)
        {
          AjaxResult  retus = null;
           var cou= dbtext.GetList().Where(a => a.ClassNumber == classSchedule.ClassNumber && a.IsDelete == false &&a.ClassStatus==false).Count();
            if (cou>0)
            {
                retus = new ErrorResult();
                retus.Msg = "班级名称重复";

                retus.Success = false;
                retus.ErrorCode = 300;
            }
            else
            {
                try
                {
                    classSchedule.ClassStatus = false;
                    classSchedule.IsDelete = false;
                    dbtext.Insert(classSchedule);
                    retus = new SuccessResult();
                    retus.Success = true;
                    retus.Msg = "开设成功";
                    BusHelper.WriteSysLog("班级开设成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
                catch (Exception ex)
                {
                    retus = new ErrorResult();
                    retus.Msg = "服务器错误";

                    retus.Success = false;
                    retus.ErrorCode = 500;
                    BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
               

                }
            }
            
            return Json(retus, JsonRequestBehavior.AllowGet);
        }
        //查看班级的学员
        public ActionResult ClassStudent()
        {

            classNumberss = Request.QueryString["ClassNumber"];
            var x = dbtext.ClassStudentneList(classNumberss);
            ViewBag.ClassName = classNumberss;
            ViewBag.ClassdetailsView = dbtext.Listdatails(classNumberss);
            ViewBag.Members = dbtext.MembersList();
            return View(x);
        }

        //群号操作页面
        public ActionResult Groupnumber()
        {
            string ClassNumber = Request.QueryString["ClassNumber"];
            ViewBag.ClassNumber = ClassNumber;
            return View(dbtext.Grouselect(ClassNumber));
        }
        //群号数据操作
        public ActionResult GroupEntity(GroupManagement groupManagement)
        {
            return Json(dbtext.GroupAdd(groupManagement), JsonRequestBehavior.AllowGet);
        }
        public ActionResult text()
        {
            return View();
        }
        //获取班委
        public ActionResult DateMembers()
        {
            return Json(dbtext.ClassStudentneList(classNumberss), JsonRequestBehavior.AllowGet);
        }
        //班委数据操作
        public ActionResult AddMembers()
        {
            //学员学号
            string Stuid = Request.QueryString["Stuid"];
            //班委名称
            string MenName = Request.QueryString["MenName"];
            //数据操作
            string Entity = Request.QueryString["Entity"];


         return Json(dbtext.Entityembers(Stuid, MenName, classNumberss, Entity),JsonRequestBehavior.AllowGet);



        }
        //班会表单页面
        [HttpGet]
        public ActionResult AssmeetingsEntity()
        {
            //undefined
            //string ClassName = Request.QueryString["ClassName"];

            string uid = Request.QueryString["id"];
            Assmeetings assmeetings = new Assmeetings();
            if (uid!= "undefined")
            {
                ViewBag.Name = "添加班会记录";
                assmeetings =  dbtext.AssmeetingsSelect(int.Parse(uid));
                return View(assmeetings);
            }
            else
            {
                ViewBag.Name = "编辑班会记录";
                assmeetings.ClassNumber = classNumberss;
                return View(assmeetings);
            }
         
        }
        //班会数据操作方法
        [HttpPost]
        public ActionResult AssmeetingsEntity(Assmeetings assmeetings)
        {
            return Json(dbtext.EntityAssmeetings(assmeetings), JsonRequestBehavior.AllowGet);
        }

        //查看是否有相同的班委
        public ActionResult AssmeetingsBool()
        {
            //班委名称
            string MenName = Request.QueryString["MenName"];
            //学号
            string Stuid = Request.QueryString["Stuid"];
            return Json(dbtext.AssmeetingsBool(MenName, classNumberss, Stuid), JsonRequestBehavior.AllowGet);
        }
        //班级班会数据
        public ActionResult AssmeetingsGetDate(int page, int limit,string Title,string qBeginTime,string qEndTime)
        {
            var dataList = dbtext.AssmeetingsList(classNumberss) ;
            if (!string.IsNullOrEmpty(Title))
            {
                dataList = dataList.Where(a => a.Title.Contains(Title)).ToList();
            }
       
            if (!string.IsNullOrEmpty(qBeginTime))
            {
                dataList = dataList.Where(a => a.Classmeetingdate >= Convert.ToDateTime(qBeginTime)).ToList();
            }
            if (!string.IsNullOrEmpty(qEndTime))
            {
                dataList = dataList.Where(a => a.Classmeetingdate <= Convert.ToDateTime(qEndTime)).ToList();
            }
            dataList= dataList.OrderBy(a => a.ID).Skip((page - 1) * limit).Take(limit).ToList();
            var data = new
            {
                code = "",
                msg = "",
                count = dataList.Count,
                data = dataList
            }; return Json(data, JsonRequestBehavior.AllowGet);
        
          
       
        }
    }
}