using SiliconValley.InformationSystem.Business.Employment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    /// <summary>
    /// 总结
    /// </summary>
    public class EmploymentSummaryController : Controller
    {
        private QuarterBusiness dbquarter;
        // GET: Obtainemployment/EmploymentSummary
        public ActionResult EmploymentSummaryIndex()
        {

            return View();
        }

        /// <summary>
        /// 加载树
        /// </summary>
        /// <returns></returns>
        public ActionResult EstablishTree()
        {

            dbquarter = new QuarterBusiness();
            var result = dbquarter.loadtree();
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 加载员工
        /// </summary>
        /// <returns></returns>
        public ActionResult loadempstaff() {
            return null;
        }
    }
}