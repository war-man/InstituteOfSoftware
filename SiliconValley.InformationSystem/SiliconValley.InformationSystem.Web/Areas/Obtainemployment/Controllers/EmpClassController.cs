﻿using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Util;
    public class EmpClassController : Controller
    {
        /// <summary>
        /// 就业专员带班记录业务类
        /// </summary>
        private EmpClassBusiness dbempclass;
        /// <summary>
        /// 就业专员业务类
        /// </summary>
        private EmploymentStaffBusiness dbempstaff;
        /// <summary>
        /// 方向专业业务类
        /// </summary>
        private SpecialtyBusiness dbspee;
        /// <summary>
        /// 就业班级管理
        /// </summary>
        /// <returns></returns>
        public ActionResult EmpClassIndex()
        {

            return View();
        }

        
        /// <summary>
        /// 展示数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult SearchEmpclassInfo(int page, int limit, string empname, string classnumber, string whyshow)
        {
            dbempclass = new EmpClassBusiness();
            dbempstaff = new EmploymentStaffBusiness();
            dbspee = new SpecialtyBusiness();
            //获取了s3以s4未毕业的班级而且没分配的班级
            var NoGraduation = dbempclass.NoDistribution();
            //获取了专员带班记录表
            var empclasslist = dbempclass.GetEmpClassFormServer().OrderByDescending(a => a.EmpStaffID).ToList();
            var emplist = empclasslist.Select(a => new EmpClassView
            {
                ClassNumber = a.ClassNO,
                EmpName = dbempstaff.GetEmpInfoByEmpID(a.EmpStaffID).EmpName,
                Phone = dbempstaff.GetEmpInfoByEmpID(a.EmpStaffID).Phone,
                EntID = a.EmpStaffID,
                GrandName = dbempclass.GetGrandByClassNo(a.ClassNO).GrandName,
                empclassDate = a.dirDate,
                Remark = a.Remark,
                SpecialtyName = dbspee.GetSpecialtyByID(dbempclass.GetClassingByID(a.ClassNO).Major_Id).SpecialtyName
            }).ToList();
            var noemplist = NoGraduation.Select(a => new EmpClassView
            {
                ClassNumber = a.ClassNumber,
                EmpName = "",
                Phone = "",
                EntID = null,
                GrandName = dbempclass.GetGrandByID(a.grade_Id).GrandName,
                empclassDate = null,
                Remark = "",
                SpecialtyName = dbspee.GetSpecialtyByID(a.Major_Id).SpecialtyName
            }).ToList();
            var resultdata = new List<EmpClassView>();
            switch (whyshow)
            {
                case "distribution":
                    resultdata = emplist;
                    break;
                case "nodistribution":
                    resultdata = noemplist;
                    break;
                default:
                    noemplist.AddRange(emplist);
                    resultdata = noemplist;
                    break;
            }
            if (!string.IsNullOrEmpty(empname))
            {
                resultdata = resultdata.Where(a => a.EmpName.Contains(empname)).ToList();
            }
            if (!string.IsNullOrEmpty(classnumber))
            {
                resultdata = resultdata.Where(a => a.ClassNumber.Contains(classnumber)).ToList();
            }
            var bnewdata = resultdata.Skip((page - 1) * limit).Take(limit).ToList();
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = noemplist.Count(),
                data = bnewdata
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Distribution(string id)
        {
            dbempclass = new EmpClassBusiness();
            dbempstaff = new EmploymentStaffBusiness();
            var resultclass = dbempclass.GetClassingByID(id);
            var empstaffdata = dbempstaff.GetALl();
            var resultdata = empstaffdata.Select(a => new SelectListItem
            {
                Text = dbempstaff.GetEmployeesInfoByID(a.EmployeesInfo_Id).EmpName,
                Value = a.ID.ToString()
            }).ToList();
            ViewBag.EmpStaff = resultdata;
            List<PiechartView> mypie = new List<PiechartView>();
            PiechartView pie = new PiechartView();
            pie.count = 100;
            pie.Corlor = "red";
            pie.showname = "小白";
            PiechartView pie1 = new PiechartView();
            pie1.count = 500;
            pie1.Corlor = "#b6a2dd";
            pie1.showname = "小弟弟";
            PiechartView pie2 = new PiechartView();
            pie2.count = 10;
            pie2.Corlor = "#2dc6c8";
            pie2.showname = "臭美没";
            PiechartView pie3 = new PiechartView();
            pie3.count = 300;
            pie3.Corlor = "#d7797f";
            pie3.showname = "闹可能";
            mypie.Add(pie);
            mypie.Add(pie1);
            mypie.Add(pie2);
            mypie.Add(pie3);
            ViewBag.pielist = Newtonsoft.Json.JsonConvert.SerializeObject(mypie);
            return View(resultclass);
        }

        public ActionResult GetClassList(int empstaffid)
        {
            dbempclass = new EmpClassBusiness();
            var empclasslist = dbempclass.GetEmpsByEmpID(empstaffid);
            var classinglist = dbempclass.GetClassingList(empclasslist);
            return Json(classinglist, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClassToEmpstaff(string ClassNO,int empstaffid) {
            dbempclass = new EmpClassBusiness();
            EmpClass empClass = new EmpClass();
            empClass.ClassNO = ClassNO;
            empClass.dirDate = DateTime.Now;
            empClass.EmpStaffID = empstaffid;
            empClass.IsDel = false;
            empClass.Remark = "带班";
            empClass.StartTime = DateTime.Now;
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbempclass.Insert(empClass);
                ajaxResult.Data = "";
                ajaxResult.ErrorCode = 200;
                ajaxResult.Msg = "分配成功";
                ajaxResult.Success = true;

            }
            catch (Exception ex)
            {

                ajaxResult.Data = "";
                ajaxResult.ErrorCode = 200;
                ajaxResult.Msg = ex.Message;
                ajaxResult.Success = true;
            }
            return Json(ajaxResult,JsonRequestBehavior.AllowGet);
        }
    }
}