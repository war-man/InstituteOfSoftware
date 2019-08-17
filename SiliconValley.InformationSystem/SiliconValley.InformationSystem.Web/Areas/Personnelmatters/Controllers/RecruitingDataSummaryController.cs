using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    using SiliconValley.InformationSystem.Business.RecruitingDataSummaryBusiness;
    public class RecruitingDataSummaryController : Controller
    {
        // GET: Personnelmatters/RecruitingDataSummary
        public ActionResult RecruitIndex()
        {
            return View();
        }
        public ActionResult GetRecruitData(int page,int limit) {
            RecruitingDataSummaryManage rdsmanage = new RecruitingDataSummaryManage();
            var rdslist = rdsmanage.GetList();
            var myrdslist = rdslist.OrderBy(r => r.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var newobj = new
            {
                code = 0,
                msg = "",
                count = rdslist.Count(),
                data = rdslist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }
    }
}