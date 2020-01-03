using SiliconValley.InformationSystem.Business.Psychro;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{

    [CheckLogin]

    /// <summary>
    /// 找生年计划控制器
    /// </summary>
    public class SchoolYearPlanController : Controller
    {
        /// <summary>
        /// 学校招生计划业务类
        /// </summary>
        private SchoolYearPlanBusiness dbplan;
        // GET: Market/SchoolYearPlan


        /// <summary>
        /// 显示年度计划列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SchoolYearPlanIndex() {
            return View();
        }


        /// <summary>
        /// 计划数据
        /// </summary>
        /// <returns></returns>
        public ActionResult SearchSchoolYearPlan(int page, int limit) {
            dbplan = new SchoolYearPlanBusiness();
            var getdata= dbplan.GetAll();
            var resultdata= getdata.OrderByDescending(a=>a.ID).Skip((page - 1) * limit).Take(limit).ToList();
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = getdata.Count(),
                data = resultdata
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// get 请求页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddSchoolYearPlan() {
            return View();
        }

        /// <summary>
        ///post 添加数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddSchoolYearPlan(SchoolYearPlan formdata) {
            dbplan = new SchoolYearPlanBusiness();
           var oldlist=  dbplan.GetAll();
           var datatimenoe = DateTime.Now;
            bool isadd = false;
            AjaxResult ajaxResult = new AjaxResult();
            foreach (var item in oldlist)
            {
                if (item.PlanDate.Year.ToString()==datatimenoe.Year.ToString())
                {
                    isadd = true;
                }
            }
            if (isadd)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "已经计划好了今年的计划了。";
                ajaxResult.ErrorCode = 200;
                ajaxResult.Data = string.Empty;
            }
            else
            {
                formdata.IsDel = false;
                formdata.PlanDate = datatimenoe;
                
                try
                {
                    dbplan.Insert(formdata);
                    ajaxResult = dbplan.Success("添加成功");

                }
                catch (Exception ex)
                {

                    ajaxResult = dbplan.Error(ex.Message);
                }
            }
            
            return Json(ajaxResult,JsonRequestBehavior.AllowGet) ;
        }


        [HttpGet]
        /// <summary>
        /// 修改计划 只能修改当前这年得计划。
        /// 不能修改其他年份得计划
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public ActionResult EidtSchoolYearPlan(int param0)
        {
            dbplan = new SchoolYearPlanBusiness();
            SchoolYearPlan query = dbplan.GetEntity(param0);
            ViewBag.data = Newtonsoft.Json.JsonConvert.SerializeObject(query);
            return View();

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EidtSchoolYearPlan(SchoolYearPlan param0)
        {
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbplan = new SchoolYearPlanBusiness();
                SchoolYearPlan querydata=  dbplan.GetEntity(param0.ID);
                querydata.AreaNumber = param0.AreaNumber;
                querydata.Estimatednumberofwordofmouth = param0.Estimatednumberofwordofmouth;
                querydata.PredictedNetworkNumber = param0.PredictedNetworkNumber;
                querydata.Remak = param0.Remak;
                querydata.Title = param0.Title;
                dbplan.Update(querydata);
                ajaxResult.Success = true;
            }
            catch (Exception ex)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请联系信息部成员！";
            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);

        }
    }
}