using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    public class EmpStaffAndStuController : Controller
    {
        private EmpStaffAndStuBusiness dbempStaffAndStu;
        private EmploymentAreasBusiness dbemploymentAreas;
        // GET: Obtainemployment/EmpStaffAndStu
        public ActionResult EmpStaffAndStuIndex()
        {
            dbemploymentAreas = new EmploymentAreasBusiness();
            var list1= dbemploymentAreas.GetAll();
            ViewBag.arealist = list1;
            ViewBag.firstid = list1.FirstOrDefault().ID;
            return View();
        }
        /// <summary>
        /// 数据表格
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="string0">地区id</param>
        /// <param name="string1">学生编号</param>
        /// <returns></returns>
        public ActionResult table00(int page, int limit, int string0, string string1)
        {
            dbempStaffAndStu = new EmpStaffAndStuBusiness();
           List< EmpStaffAndStuView> data =  dbempStaffAndStu.Getnodistribution();
           data= data.Where(a => a.AreaID == string0).ToList();
            if (!string.IsNullOrEmpty(string1))
            {
                List<string> studentno = string1.Split('-').ToList();
                for (int i = data.Count - 1; i >= 0; i--)
                {
                    for (int j = 0; j < data.Count; j++)
                    {
                        if (data[i].StudentNO.ToString() != studentno[j])
                        {
                            if (j == studentno.Count - 1)
                            {
                                data.Remove(data[i]);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            var data1 = data.OrderByDescending(a => a.EmploymentStage).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = data.Count(),
                data = data1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

    }
}