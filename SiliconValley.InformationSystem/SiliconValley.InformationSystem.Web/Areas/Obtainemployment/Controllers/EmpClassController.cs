using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
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
        /// 就业班级管理
        /// </summary>
        /// <returns></returns>
        public ActionResult EmpClassIndex()
        {
            dbempstaff = new EmploymentStaffBusiness();
              var empstaffdata=  dbempstaff.GetALl();
          var resultdata=  empstaffdata.Select(a => new SelectListItem
            {
                Text = dbempstaff.GetEmployeesInfoByID(a.EmployeesInfo_Id).EmpName,
                Value = a.ID.ToString()
            }).ToList();
            ViewBag.EmpStaff = resultdata;
            return View();
        }

        /// <summary>
        /// redis 返回empclass集合
        /// </summary>
        /// <returns></returns>
        public List<EmpClass> RedisGetEmpClassList() {
            RedisCache myredis = new RedisCache();
            var resultlist = new List<EmpClass>();
            resultlist = myredis.GetCache<List<EmpClass>>("redistoempclass");
            if (resultlist.Count!=0)
            {
                return resultlist;
            }
            else
            {
                //清除缓存 当数据修改时进行清除
                //myredis.RemoveCache("redistoempclass");
                //拿数据库的数据
                dbempclass = new EmpClassBusiness();
                var empclassdata= dbempclass.GetEmpClass();
                //放入缓存中
                myredis.SetCache("redistoempclass", empclassdata);
                return empclassdata;
            }
        }
        /// <summary>
        /// 展示数据
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult SearchEmpclassInfo(int page, int limit,string empname,string classnumber,string whyshow)
        {
            dbempclass = new EmpClassBusiness();
            dbempstaff = new EmploymentStaffBusiness();
            //获取了s3以s4未毕业的班级而且没分配的班级
            var NoGraduation = dbempclass.NoDistribution();
            //获取了专员带班记录表
            var empclasslist = dbempclass.GetEmpClass().OrderByDescending(a => a.EmpStaffID).ToList();
            var emplist = empclasslist.Select(a => new EmpClassView
            {
                ClassNumber = a.ClassNO,
                EmpName = dbempstaff.GetEmpInfoByEmpID(a.EmpStaffID).EmpName,
                Phone = dbempstaff.GetEmpInfoByEmpID(a.EmpStaffID).Phone,
                EntID = a.EmpStaffID,
                GrandName = dbempclass.GetGrandByClassNo(a.ClassNO).GrandName,
                empclassDate = a.Date,
                Remark = a.Remark
            }).ToList();
            var noemplist = NoGraduation.Select(a => new EmpClassView
            {
                ClassNumber = a.ClassNumber,
                EmpName = "",
                Phone = "",
                EntID = null,
                GrandName = dbempclass.GetGrandByID(a.grade_Id).GrandName,
                empclassDate = null,
                Remark = ""
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
                resultdata= resultdata.Where(a => a.EmpName.Contains(empname)).ToList();
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


        public ActionResult Distribution(string ClassNumber) {
            dbempclass = new EmpClassBusiness();
           var resultclass=  dbempclass.GetClassingByID(ClassNumber);
            return View();
        }
    }
}