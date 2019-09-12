using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Business.StudentmanagementBusinsess;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Web.Areas.Teachingquality.Controllers
{
    public class PricedetailsController : Controller
    {
        private readonly StudentFeeStandardBusinsess dbtext;
        public PricedetailsController()
        {
            dbtext = new StudentFeeStandardBusinsess();
        }
        // GET: Teachingquality/Pricedetails
        //专业
        SpecialtyBusiness Techarcontext = new SpecialtyBusiness();
        //阶段
        GrandBusiness Grandcontext = new GrandBusiness();
        public ActionResult Index()
        {
            return View();
        }
        //获取学费标价数据
        public ActionResult StudentFeeList(int page, int limit)
        {
            return Json(dbtext.StudentFeeList(page, limit), JsonRequestBehavior.AllowGet);
        }
        //录入学员学费详情单
        [HttpGet]
        public ActionResult AddFeeStudent()
        {
            //专业
            ViewBag.Major_Id = Techarcontext.GetList().Select(a => new SelectListItem { Text = a.SpecialtyName, Value = a.Id.ToString() });
            //阶段
            ViewBag.Grand_Id = Grandcontext.GetList().Select(a => new SelectListItem { Text = a.GrandName, Value = a.Id.ToString() });
            return View();
        }
        //查询根据阶段专业查询数据库是否存在，不存在则录入
        public bool BoolFeeStude()
        {
            int Grand_Id = Convert.ToInt32(Request.QueryString["Grand_Id"]);
            int Major_Id =Convert.ToInt32( Request.QueryString["Major_Id"]);
            return dbtext.BoolFeeStude(Grand_Id, Major_Id);
        }
        //录入学员学费详情单数据操作
        [HttpPost]
        public ActionResult AddFeeStudent(decimal Foodandlodging, decimal Tuition, int Grand_Id, int Major_Id)
        {
            var x = dbtext.AddFeeStudent(Foodandlodging, Tuition, Grand_Id, Major_Id);
            return Json(x, JsonRequestBehavior.AllowGet);
        }

    }
}