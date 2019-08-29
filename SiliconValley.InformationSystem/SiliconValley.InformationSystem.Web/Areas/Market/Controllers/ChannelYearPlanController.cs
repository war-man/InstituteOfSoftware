using SiliconValley.InformationSystem.Business.Psychro;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class ChannelYearPlanController : Controller
    {
        private SchoolYearPlanBusiness dbschoolpaln;
        // GET: Market/ChannelYearPlan
        public ActionResult ChannelYearPlanIndex()
        {
            dbschoolpaln = new SchoolYearPlanBusiness();
            ViewBag.YearName = dbschoolpaln.GetAll().Select(a => new ShowyearnameView
            {
                SchoolPlanID = a.ID,
                YearName = a.PlanDate.Year.ToString() + "年"
            });
            ViewBag.NowYearName = DateTime.Now.Year.ToString() + "年";
            return View();
        }
    }
}