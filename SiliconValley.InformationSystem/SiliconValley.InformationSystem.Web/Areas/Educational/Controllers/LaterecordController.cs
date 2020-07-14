using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.TM_Data.MyViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Entity.ViewEntity.TM_Data;
using System.Text;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    [CheckLogin]
    public class LaterecordController : Controller
    {
        private LaterecordManeger leatercord_Entity = new LaterecordManeger();
        // GET: /Educational/Laterecord/EditView
        public ActionResult LaterecordIndex()
        {
            List<SelectListItem> g_list = new List<SelectListItem>() { new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true } };
            g_list.AddRange(Reconcile_Com.GetGrand_Id().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList());

            ViewBag.grandlist_S = g_list;
            return View();
        }


        public ActionResult GetTableData(int limit,int page)
        {
            Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();//获取登录人信息
            List<LaterecordView> list = leatercord_Entity.GetallView();
            if (!leatercord_Entity.IsShowAll(UserName.EmpNumber))
            {
                list = list.Where(l => l.CreateUser == UserName.EmpNumber).OrderByDescending(l => l.Id).ToList();
            }
              
            var data = list.Skip((page - 1) * limit).Take(limit).Select(l => new {
                Id = l.Id,
                IsHavaHeadMaster = l.IsHavaHeadMaster == true ? "是" : "否",
                IshavaPPT = l.IshavaPPT == true ? "是" : "否",
                IsHavaTeacher = l.IsHavaTeacher == true ? "是" : "否",
                PersonCount = l.PersonCount,
                PctualCout=l.PctualCout,
                Createdate=l.Createdate,
                EmpName = l.EmpName,
                ClassNumber=l.ClassNumber,
                Reak=l.Reak,
            });

            var jsondata = new { code=0,data=data,count=list.Count};
            return Json(jsondata,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用于模糊查询
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult SercherData(int limit, int page)
        {
            string class_S = Request.QueryString["class_S"];
            string SatrTime = Request.QueryString["SatrTime"];
            string EndTime = Request.QueryString["EndTime"];
            Base_UserModel UserName = Base_UserBusiness.GetCurrentUser();//获取登录人信息
            StringBuilder sb = new StringBuilder("select * from LaterecordView where 1=1");
            if (!leatercord_Entity.IsShowAll(UserName.EmpNumber))
            {
                sb.Append(" and CreateUser='"+ UserName.EmpNumber + "'");
            }
                 
            if (!string.IsNullOrEmpty(class_S) && class_S!="0")
            {
                sb.Append(" and Class_Id="+class_S);
            }

            if (!string.IsNullOrEmpty(SatrTime))
            {
                DateTime da1 = Convert.ToDateTime(SatrTime);
                sb.Append(" and CreateDate>=" + da1);
            }

            if (!string.IsNullOrEmpty(EndTime))
            {
                DateTime da1 = Convert.ToDateTime(EndTime);
                sb.Append(" and CreateDate<=" + da1);
            }

            List<LaterecordView> list= leatercord_Entity.GetListBySql<LaterecordView>(sb.ToString());

            var data = list.Skip((page - 1) * limit).Take(limit).Select(l => new {
                Id = l.Id,
                IsHavaHeadMaster = l.IsHavaHeadMaster == true ? "是" : "否",
                IshavaPPT = l.IshavaPPT == true ? "是" : "否",
                IsHavaTeacher = l.IsHavaTeacher == true ? "是" : "否",
                PersonCount = l.PersonCount,
                PctualCout = l.PctualCout,
                Createdate = l.Createdate,
                EmpName = l.EmpName,
                ClassNumber = l.ClassNumber,
                Reak = l.Reak,
            });

            var jsondata = new { code = 0, data = data, count = list.Count };
            return Json(jsondata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult insertView()
        {
            //加载阶段
            List<SelectListItem> g_list = new List<SelectListItem>() { new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true } };
            g_list.AddRange(Reconcile_Com.GetGrand_Id().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList());

            ViewBag.grandlist = g_list;
            return View();
        }

        /// <summary>
        /// 获取班级人数
        /// </summary>
        /// <returns></returns>
        public ActionResult ClassCount(int id)
        {
            string str = "select* from ScheduleForTraineesview where Classid =" + id;
            AjaxResult a = new AjaxResult();
            a.Data= leatercord_Entity.GetListBySql<ScheduleForTraineesview>(str).Count;

            return Json(a,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="newdata"></param>
        /// <returns></returns>
        public ActionResult Addfunction(Laterecord newdata)
        {
            newdata.Createdate = DateTime.Now;
            newdata.CreateUser = Base_UserBusiness.GetCurrentUser().EmpNumber;

            AjaxResult a= leatercord_Entity.Add_data(newdata);

            return Json(a,JsonRequestBehavior.AllowGet);
        }


        public ActionResult EditView(int id)
        {

            //加载阶段
            List<SelectListItem> g_list = new List<SelectListItem>() { new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true } };
            g_list.AddRange(Reconcile_Com.GetGrand_Id().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList());

            ViewBag.grandlist = g_list;
            LaterecordView findata= leatercord_Entity.FindSingData(id);
            return View(findata);
        }

        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="newdata"></param>
        /// <returns></returns>
        public ActionResult EditFuntion(Laterecord newdata)
        {
            Laterecord laterecord = leatercord_Entity.GetEntity(newdata.Id);
            laterecord.IsHavaHeadMaster = newdata.IsHavaHeadMaster;
            laterecord.IshavaPPT = newdata.IshavaPPT;
            laterecord.IsHavaTeacher = newdata.IsHavaTeacher;
            laterecord.PctualCout = newdata.PctualCout;
            laterecord.Reak = newdata.Reak;

            AjaxResult a= leatercord_Entity.update_data(laterecord);


            return Json(a,JsonRequestBehavior.AllowGet);

        }


    }
}