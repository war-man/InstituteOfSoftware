using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    //[CheckLogin]
    /// <summary>
    /// 总结
    /// </summary>
    public class EmploymentSummaryController : Controller
    {
        private QuarterBusiness dbquarter;
        private EmpStaffAndStuBusiness dbempStaffAndStu;
        private EmploymentStaffBusiness dbEmploymentStaff;
        private EmployeesInfoManage dbemployeesInfoManage;
        // GET: Obtainemployment/EmploymentSummary
        public ActionResult EmploymentSummaryIndex()
        {

            return View();
        }

        /// <summary>
        /// 加载树
        /// </summary>
        /// <returns></returns>
        public ActionResult EstablishTree()
        {

            dbquarter = new QuarterBusiness(); 
            var result = dbquarter.loadtree();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 加载员工
        /// </summary>
        /// <param name="param0">等级，1为年，2为季度</param>
        /// <param name="param1">值 2019 或者 1007</param>
        /// <returns></returns>
        public ActionResult loadempstaff(bool param0,int param1) {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbEmploymentStaff = new EmploymentStaffBusiness();
                dbemployeesInfoManage = new EmployeesInfoManage();
                var data = dbEmploymentStaff.GetSummaryStaffs(param0, param1);
                ajaxResult.Data = data.Select(a => new {
                    a.ID,
                    staffname = dbemployeesInfoManage.GetEntity(a.EmployeesInfo_Id).EmpName
                }).ToList();
                ajaxResult.Success = true;
            }
            catch (Exception ex)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 就业率
        /// </summary>
        /// <returns></returns>
        public ActionResult EmploymentRatio()
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Year"></param>
        /// <param name="type">already， not</param>
        /// <returns></returns>
        public ActionResult YearEmploymentRatioData(string Year, string type, int page, int limit)
        {
            dbempStaffAndStu = new EmpStaffAndStuBusiness();

            var alllist = dbempStaffAndStu.EmploymentRatio(Year:Year);

            int count = 0;

            List<EmpStaffAndStuView> resultlist = RatioData(alllist, type, page, limit, ref count);

            var obj = new {
                code = 0,
                msg = "",
                count = count,
                data = resultlist
            };

            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ClassEmploymentRatioData(string classid, string type, int page, int limit)
        {
            dbempStaffAndStu = new EmpStaffAndStuBusiness();
            var alllist = dbempStaffAndStu.EmploymentRatio(classid:int.Parse(classid));

            int count = 0;

            List<EmpStaffAndStuView> resultlist = RatioData(alllist, type, page, limit, ref count);

            var obj = new
            {
                code = 0,
                msg = "",
                count = count,
                data = resultlist
            };

            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        public ActionResult MajorEmploymentRatioData(string majorid,string Year, string type, int page, int limit)
        {
            dbempStaffAndStu = new EmpStaffAndStuBusiness();

            var alllist = dbempStaffAndStu.EmploymentRatio(majorid:int.Parse(majorid), Year:Year);

            int count = 0;

            List<EmpStaffAndStuView> resultlist = RatioData(alllist, type, page, limit, ref count);

            var obj = new
            {
                code = 0,
                msg = "",
                count = count,
                data = resultlist
            };

            return Json(obj, JsonRequestBehavior.AllowGet);

        }

        public ActionResult StuffEmploymentRatioData(string empnumber, string Year, string type, int page, int limit)
        {

            //EmpStaffAndStu
            dbempStaffAndStu = new EmpStaffAndStuBusiness();
            dbEmploymentStaff = new EmploymentStaffBusiness();
            var stuff = dbEmploymentStaff.GetList().Where(d => d.EmployeesInfo_Id == empnumber).FirstOrDefault();
            var alllist = dbempStaffAndStu.EmploymentRatio(Year:Year, StaffId: stuff.ID.ToString());

            int count = 0;

            List<EmpStaffAndStuView> resultlist = RatioData(alllist, type, page, limit, ref count);

            var obj = new
            {
                code = 0,
                msg = "",
                count = count,
                data = resultlist
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public List<EmpStaffAndStuView> RatioData(List<EmpStaffAndStu> sourse, string type, int page, int limit, ref int count)
        {
            List<EmpStaffAndStu> templist = new List<EmpStaffAndStu>();

            switch (type)
            {
                case "not":
                    templist = sourse.Where(d => d.EmploymentState == 1 || d.EmploymentState == 3).ToList();
                    break;
                case "already":
                    templist = sourse.Where(d => d.EmploymentState == 2).ToList();
                    break;

            }

            count = templist.Count;

            List<EmpStaffAndStu> skiplist = templist.Skip((page - 1) * limit).Take(limit).ToList();

            List<EmpStaffAndStuView> datalist = new List<EmpStaffAndStuView>();

            foreach (var item in skiplist)
            {
                var tempobj = dbempStaffAndStu.studentnoconversionempstaffandstubiew(item.Studentno);

                if (tempobj != null)

                    datalist.Add(tempobj);
            }

            return datalist;
        }


        /// <summary>
        /// 获取毕业班级
        /// </summary>
        /// <returns></returns>
        public ActionResult GraduationClass()
        {
            EmpClassBusiness db_empclass = new EmpClassBusiness();

            //毕业班级
           var classlist = db_empclass.GetClassFormServer().Where(d=>d.ClassStatus == true).ToList();

            var db_major = new SpecialtyBusiness();

            List<object> templist = new List<object>();

            //var tempobj1 = new
            //{

            //    classname = "1710",
            //    classid ="11",
            //    major = "java",
            //    heammaster ="杨雪",
            //    graduationDate = "2020/02/03" 

            //};

            //templist.Add(tempobj1);
            foreach (var item in classlist)
            {
                var tempemp = new EmployeesInfoManage();
                var stuffemp = db_empclass.GetStaffByClassid(item.id);
                var emp = tempemp.GetInfoByEmpID(stuffemp.EmployeesInfo_Id);


                var tempobj = new {

                    classname = item.ClassNumber,
                    classid = item.id,
                    major = db_major.GetSpecialties().Where(d => d.Id == item.Major_Id).FirstOrDefault().SpecialtyName,
                    heammaster = emp.EmpName,
                    graduationDate = db_empclass.GetEmpClassFormServer().Where(d=>d.ClassId == item.id).FirstOrDefault().EndingTime

                };

                templist.Add(tempobj);
            }

            var obj = new {
                code = 0,
                msg = "",
                count = classlist.Count,
                data = templist

            };

            return Json(obj, JsonRequestBehavior.AllowGet);
           
        }

        public ActionResult selectGraduationClass()
        {
            return View();
        }

        public ActionResult selectMajor()
        {
            var db_major = new SpecialtyBusiness();
            ViewBag.majors = db_major.GetSpecialties();
            return View();
        }

        public ActionResult selectStuff()
        {
            return View();
        }

        public ActionResult StuffData(int page, int limit , string stuffName = null)
        {
            dbEmploymentStaff = new EmploymentStaffBusiness();

            List<EmployeesInfo> list = new List<EmployeesInfo>();

            List<EmployeesInfo> resutllist = new List<EmployeesInfo>();

            var templist = dbEmploymentStaff.GetList();

            foreach (var item in templist)
            {
                var tempempobj = dbEmploymentStaff.GetEmployeesInfoByID(item.EmployeesInfo_Id);

                if (tempempobj != null)
                    list.Add(tempempobj);
            }

            if (string.IsNullOrEmpty(stuffName))
            {
                if (list != null)
                    resutllist.AddRange(list);
            }

            else
            {
                if (list != null)
                {
                   resutllist.AddRange( list.Where(d => d.EmpName.Contains(stuffName)).ToList());
                }
                    
            }

            var obj = new {

                code = 0,
                msg = "",
                count = resutllist.Count,
                data = resutllist.Skip((page - 1) * limit).Take(limit).ToList()
            };

            return Json(obj, JsonRequestBehavior.AllowGet);

        }

       

    }
}