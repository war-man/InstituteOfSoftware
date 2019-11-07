using SiliconValley.InformationSystem.Business.EducationalBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    public class ReconcileController : Controller
    {
        // GET: /Educational/Reconcile/GetClassScheduleSelect

        public static readonly ReconcileManeger Reconcile_Entity = new ReconcileManeger();

        public ActionResult ReconcileIndexViews()
        {
            //加载所有阶段
            List<SelectListItem> g_list = Reconcile_Entity.GetEffectiveData().Select(g => new SelectListItem() { Text = g.GrandName, Value = g.Id.ToString() }).ToList();
            g_list.Add(new SelectListItem() { Text = "--请选择--", Value = "0", Selected = true });
            ViewBag.grandlist = g_list;
            return View();
        }
        /// <summary>
        /// 通过阶段获取班级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetClassScheduleSelect(int id)
        {
           var c_list= Reconcile_Entity.GetGrandClass(id).ToList();
            return Json(c_list,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 通过班级名称获取班级其他数据
        /// </summary>
        /// <param name="id">班级名称</param>
        /// <returns></returns>
        public ActionResult GetClassDate(string id)
        {
            ClassSchedltData new_c = new ClassSchedltData();
           ClassSchedule find_c= ReconcileManeger.ClassSchedule_Entity.GetEntity(id);
            new_c.Name = find_c.ClassNumber;//班级名称
           string marjon=  ReconcileManeger.ClassSchedule_Entity.GetClassGrand(find_c.ClassNumber,1);//专业
            new_c.marjoiName = marjon;
           string grand = ReconcileManeger.ClassSchedule_Entity.GetClassGrand(find_c.ClassNumber, 2);//阶段
            new_c.GrandName = grand;
           string time = ReconcileManeger.ClassSchedule_Entity.GetClassGrand(find_c.ClassNumber, 3);//上课时间类型
            new_c.ClassDate = time;         
            return Json(new_c,JsonRequestBehavior.AllowGet);
        }
    }
}