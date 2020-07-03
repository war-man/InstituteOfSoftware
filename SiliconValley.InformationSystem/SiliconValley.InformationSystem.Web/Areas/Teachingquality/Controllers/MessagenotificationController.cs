using SiliconValley.InformationSystem.Business.Messagenotification_Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    [CheckLogin]
    public class MessagenotificationController : Controller
    {
        // GET: Teachingquality/Messagenotification
        public ActionResult Index()
        {
            return View();
        }

      
        //通知数据页面
        public ActionResult NoticeDate()
        {
            return View();
        }
        MessagenotificationBusiness dbtext = new MessagenotificationBusiness();
        public ActionResult Date(int page, int limit)
        {
            return Json(dbtext.Date(page, limit), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Release()
        {
            return View();
        }
        [ValidateInput(false)]
        public ActionResult AddMessagenoti(string Duedate, string Conten, string NotifierEmployeeId)
        {
            return Json(dbtext.AddMessagenoti(Duedate, Conten, NotifierEmployeeId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DateList(string Xi)
        {
            return Json(dbtext.DateList(Xi), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改读取状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Messageread(int id)
        {
            return Json(dbtext.Messageread(id), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取单条提示信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult FinetMessag(int id)
        {
            return Json(dbtext.FinetMessag(id),JsonRequestBehavior.AllowGet);
        }
    }
}