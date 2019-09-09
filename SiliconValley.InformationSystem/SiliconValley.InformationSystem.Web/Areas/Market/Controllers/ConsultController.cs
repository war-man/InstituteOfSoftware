using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Business.Consult_Business;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class ConsultController : BaseMvcController
    {
        ConsultManeger CM_Entity = new ConsultManeger();

        // GET: /Market/Consult/FollwingInfoView
        public ActionResult ConsultIndex()
        {
            ViewBag.data = CM_Entity.GetConsultTeacher().Select(c => new ConsultShowData
            {
                empId = c.Employees_Id,
                Id = c.Id,
                empName = CM_Entity.GetEmplyeesInfo(c.Employees_Id).EmpName
            }).ToList();
            return View();
        }
        /// <summary>
        /// 显示单个咨询师的分量数据
        /// </summary>
        /// <returns></returns>
        public ActionResult SingleDataView()
        {
            return View();
        }
       /// <summary>
       /// 这是获取柱状图数据
       /// </summary>
       /// <param name="id">咨询师Id</param>
       /// <returns></returns>
        public ActionResult GetImageData(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                List<ConsultZhuzImageData> list = CM_Entity.GetImageData(null);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            else
            {
                List<ConsultZhuzImageData> list = CM_Entity.GetImageData(id);
                return Json(list, JsonRequestBehavior.AllowGet);
            }
 
        }
        /// <summary>
        /// 获取某个咨询师今年某个月完成的量和未完成的量
        /// </summary>
        /// <param name="monthName">月份</param>
        /// <param name="myid">咨询师Id</param>
        /// <returns></returns>
        public ActionResult GetTwoDivData(int MonthName, int Myid,string Staue)
        {
            int number = Staue == "完成量" ? 0 : 1;
           List<ALLDATA> list= CM_Entity.GetTeacherMonthCount(MonthName, Myid, number);
            return Json(list,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 这是跟踪未报名学生情况显示页面
        /// </summary>
        /// <returns></returns>
        public ActionResult FollwingInfoView(string id)
        {
            int stuId = Convert.ToInt32(id);
            Consult find_c= CM_Entity.TongStudentIdFindConsult(stuId);
            List<FollwingInfo> f_list = CM_Entity.GetFllowInfoData(find_c.Id);
            if (f_list.Count > 0)
            {
                ViewBag.Flowingdata = f_list;
            }
            else
            {
                FollwingInfo f = new FollwingInfo();
                f.Rank = "无";
                f.TailAfterSituation = "目前还没有跟踪信息";
                f.FollwingDate = Convert.ToDateTime("0000-00-00");
                f_list.Add(f);
                ViewBag.Flowingdata = f_list;
            }
             
            return View();
        }
    }
}