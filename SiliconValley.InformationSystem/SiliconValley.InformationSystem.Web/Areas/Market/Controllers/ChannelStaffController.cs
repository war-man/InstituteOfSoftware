using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class ChannelStaffController : Controller
    {
        // GET: Market/ChannelStaff
        public ActionResult Index()
        {
            var aa = new ChannelStaff();
           
            return View();
        }


       
    }
}