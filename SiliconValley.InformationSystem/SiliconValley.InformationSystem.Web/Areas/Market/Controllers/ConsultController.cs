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

        // GET: /Market/Consult/ListStudentView
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
        public ActionResult GetTwoDivData(int MonthName, int Myid,int Staue)
        {           
           List<ALLDATA> list= CM_Entity.GetTeacherMonthCount(MonthName, Myid, Staue);
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
            ViewBag.Flowingdata = f_list;            
            return View();
        }
        //显示查询学生信息页面
        public ActionResult SerachStudentDataView()
        {
            return View();
        }       
        //获取所有咨询师
        public ActionResult GetConsultTeacherData(int id)
        {            
                List<ConsultTeacher> list_ConsultTeacher = CM_Entity.GetConsultTeacher();//获取所有咨询师
                List<ZhuanghuanluData> list_Zhang = new List<ZhuanghuanluData>();
                foreach (ConsultTeacher item in list_ConsultTeacher)
                {                   
                    ZhuanghuanluData zhanghua = new ZhuanghuanluData();
                    zhanghua.TeacherName =CM_Entity.GetEmplyeesInfo( item.Employees_Id).EmpName;
                    int count = CM_Entity.GetCount(item.Id, id);//得到分量
                    int wangchen = CM_Entity.GetWangcenCount(item.Id, id);//得到完成量
                    if (wangchen!=0 && count!=0)
                    {
                       zhanghua.Number =Convert.ToDouble(wangchen) / Convert.ToDouble(count);
                    }else
                    {
                     zhanghua.Number = 0;
                }
                     
                    list_Zhang.Add(zhanghua);
                }
                 var data = list_Zhang.OrderByDescending(c => c.Number).ToList();
                return Json(data, JsonRequestBehavior.AllowGet);                
        }
        //查看是否有这个学生
        public ActionResult SeracherIsno(string id)
        {
            List<StudentPutOnRecord> find = CM_Entity.GetStudentPutRecored().Where(s => s.StuName == id).ToList();
            return Json(find,JsonRequestBehavior.AllowGet);
        }
        //查到多个学生的页面显示
        public ActionResult ListStudentView()
        {
            return View();
        }
    }
}