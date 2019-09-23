using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.FinaceBusines;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Business.StudentmanagementBusinsess;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Web.Areas.Finance.Controllers
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
       



        //学员费用明目页面
        public ActionResult Studentfees()
        {
          
            return View();
        }
        CostitemsBusiness costitemsBusiness = new CostitemsBusiness();
        //所有学生费用录入明目
        [HttpGet]
        public ActionResult Costitems()
        {
            //阶段
            ViewBag.Grand_id = Grandcontext.GetList().Select(a => new SelectListItem { Text = a.GrandName, Value = a.Id.ToString() });
            ViewBag.TypeDate = costitemsBusiness.TypeDate();
            return View();
        }
        [HttpPost]
        //学生费用名目录入数据操作
        public ActionResult Costitems(Costitems costitems)
        {
            return Json(costitemsBusiness.AddCostitems(costitems), JsonRequestBehavior.AllowGet);
        }
            //查询名目名称是否重复
        public int costiBoolName(int id)
        {
            string Name = Request.QueryString["Name"];
          return  costitemsBusiness.BoolName(id, Name);
        }
        //查询所有名目数据
        public ActionResult DateCostitems(int page,int limit)
        {
            return Json(costitemsBusiness.DateCostitems(page, limit), JsonRequestBehavior.AllowGet);
        }

        //学费明目类型
        [HttpGet]
        public ActionResult Typeeyesight()
        {
            return View();
        }
        //查看类别名称是否重复
        [HttpPost]
        public int TypeName(string id)
        {
            return costitemsBusiness.TypeName(id);
        }
        //添加明目类型数据操作
        public ActionResult Typeeyesight(CostitemsX costitemsX)
        {
            return Json(costitemsBusiness.AddType(costitemsX), JsonRequestBehavior.AllowGet);
        }
        //验证明目表是否有重复的数据
        public int BoolCostitems(Costitems costitems)
        {
          return  costitemsBusiness.BoolCostitems(costitems);
        }
        //学员缴费操作页面
        public ActionResult Studentpayment()
        {
            return View();
        }
        //获取学员信息
        public ActionResult GetDate(int page, int limit, string Name, string Sex, string StudentNumber, string identitydocument)
        {
            return Json(dbtext.GetDate(page, limit, Name, Sex, StudentNumber, identitydocument),JsonRequestBehavior.AllowGet);
        }
        //录入学费页面
        public ActionResult Finance()
        {
            ViewBag.Stuid = Request.QueryString["Stuid"];
            //阶段
            ViewBag.Stage = Grandcontext.GetList().Where(a => a.IsDelete == false).Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.GrandName }).ToList();
            return View();
        }





    }
}