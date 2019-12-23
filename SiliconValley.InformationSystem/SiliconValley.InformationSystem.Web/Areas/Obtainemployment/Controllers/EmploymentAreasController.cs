using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Obtainemployment.Controllers
{
    [CheckLogin]
    public class EmploymentAreasController : Controller
    {
        private EmploymentAreasBusiness dbemploymentAreas;
        // GET: Obtainemployment/EmploymentAreas
        public ActionResult EmploymentAreasIndex()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EmploymentAreasAdd()
        {

            return View();
        }

        /// <summary>
        /// 验证是否存在
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult verificationname(string param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbemploymentAreas = new EmploymentAreasBusiness();

                ajaxResult.Success = dbemploymentAreas.verificationname(param0);
            }
            catch (Exception)
            {

                ajaxResult.Success = false;
                ajaxResult.Msg = "false";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EmploymentAreasAdd(EmploymentAreas param0)
        {
            AjaxResult ajaxResult = new AjaxResult();

            try
            {
                if (!string.IsNullOrWhiteSpace(param0.AreaName))
                {
                    dbemploymentAreas = new EmploymentAreasBusiness();
                    if (dbemploymentAreas.verificationname(param0.AreaName))
                    {
                        ajaxResult.Success = false;
                        ajaxResult.Msg = "你真聪明";
                    }
                    else
                    {
                        param0.Date = DateTime.Now;
                        param0.IsDel = false;
                        param0.AreaName = param0.AreaName.Replace(" ", "");
                        dbemploymentAreas.Insert(param0);
                        ajaxResult.Success = true;
                    }

                }
                else
                {
                    ajaxResult.Success = false;
                    ajaxResult.Msg = "你真聪明";
                }

            }
            catch (Exception)
            {

                ajaxResult.Success = false;

            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult SearchData(int page, int limit)
        {
            dbemploymentAreas = new EmploymentAreasBusiness();
            var aa = dbemploymentAreas.GetIQueryable().ToList();
            var resultdata1 = aa.OrderByDescending(a => a.Date).Skip((page - 1) * limit).Take(limit).ToList();

            var returnObj = new
            {
                code = 0,
                msg = "",
                count = aa.Count(),
                data = resultdata1
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 禁用启用
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult EmploymentAreasupt(int param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbemploymentAreas = new EmploymentAreasBusiness();
                var aa = dbemploymentAreas.GetIQueryable().Where(a => a.ID == param0).FirstOrDefault();
                if (aa != null)
                {
                    if (aa.IsDel)

                        aa.IsDel = false;

                    else

                        aa.IsDel = true;
                    dbemploymentAreas.Update(aa);
                    ajaxResult.Success = true;

                }
                else
                {
                    ajaxResult.Success = false;
                }
            }
            catch (Exception)
            {
                ajaxResult.Success = false;
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
    }
}