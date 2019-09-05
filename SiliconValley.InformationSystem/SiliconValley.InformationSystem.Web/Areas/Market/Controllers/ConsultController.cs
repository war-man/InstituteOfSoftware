using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class ConsultController : BaseMvcController
    {
        // GET: Market/Consult/
        public ActionResult ConsultIndex()
        {
            return View();
        }

        //获取分量数据
        public ActionResult GetConsultData()
        {
            return null;
        }
    }
}