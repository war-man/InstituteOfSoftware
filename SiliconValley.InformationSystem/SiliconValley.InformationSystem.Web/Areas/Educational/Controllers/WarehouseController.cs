using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Educational.Controllers
{
    using SiliconValley.InformationSystem.Business.EducationalBusiness;
    using SiliconValley.InformationSystem.Entity.Entity;

    public class WarehouseController : Controller
    {
        public static readonly  WarehouseManeger Warehouse_Entity = new WarehouseManeger();
        // GET: /Educational/Warehouse/AddFunction
        public ActionResult WarehouseIndexView()
        {
            return View();
        }
        /// <summary>
        /// 获取table数据
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult GetWarehouseData(int limit,int page)
        {
            List<Warehouse> w_list= Warehouse_Entity.GetList();
            var mydata = w_list.OrderByDescending(w => w.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var jsondata = new { code = 0, msg = "", data = mydata, count = w_list.Count };

            return Json(jsondata,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AddView()
        {
            return View();
        }
        /// <summary>
        /// 添加方法
        /// </summary>
        /// <param name="new_w"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddFunction(Warehouse new_w)
        {
            new_w.Adddate = DateTime.Now;
            new_w.IsDelete = false;
            bool s = Warehouse_Entity.My_Add(new_w);
            if (s)
            {
                return Json("ok",JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("系统错误，请重试！！！", JsonRequestBehavior.AllowGet);
            }
            
        }
    }
}