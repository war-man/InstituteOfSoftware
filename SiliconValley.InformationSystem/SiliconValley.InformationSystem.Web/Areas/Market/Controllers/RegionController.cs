using SiliconValley.InformationSystem.Business.Psychro;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    [CheckLogin]
    public class RegionController : Controller
    {
        private RegionBusiness dbregion;
        // GET: Market/Region
        public ActionResult RegionIndex()
        {
            dbregion = new RegionBusiness();
            return View();
        }
        public ActionResult addregion(string param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbregion = new RegionBusiness();
                Region region = new Region();
                region.IsDel = false;
                region.RegionDate = DateTime.Now;
                region.RegionName = param0;
                dbregion.Insert(region);
                ajaxResult.Success = true;
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getregion()
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbregion = new RegionBusiness();
                ajaxResult.Data = dbregion.GetRegions();
                ajaxResult.Success = true;
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult delregion(string param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbregion = new RegionBusiness();
                List<string> delregions= param0.Split(',').ToList();
                foreach (var item in delregions)
                {
                    dbregion.Delete(item);
                }
                ajaxResult.Success = true;
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
    }
}