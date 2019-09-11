using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Teaching.Controllers
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Util;
    using SiliconValley.InformationSystem.Business.Base_SysManage;
    using SiliconValley.InformationSystem.Entity.Base_SysManage;
    using SiliconValley.InformationSystem.Business;

    /// <summary>
    /// 满意度调查控制器
    /// </summary>
    /// 
    [CheckLogin]
    public class SatisfactionSurveyController : Controller
    {


        // GET: Teaching/SatisfactionSurvey
        BaseBusiness<Department> db_dep = new BaseBusiness<Department>();

        private readonly SatisfactionSurveyBusiness db_survey;
        public SatisfactionSurveyController()
        {
            db_survey = new SatisfactionSurveyBusiness();
        }
        public ActionResult SatisfactionIndex()
        {
            return View();
        }


        /// <summary>
        /// 满意度调查配置文件
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfigSetting()
        {

            return View();
        }

        public ActionResult satisfactionItemSettingView()
        {
            //获取调查项



            //获取所有部门
            ViewBag.Department = db_dep.GetList().Where(d => d.IsDel == false).ToList();

            return View();
        }

        public ActionResult satisfactionItemTypeSettingView()
        {

            //获取所有部门
           ViewBag.Department = db_dep.GetList().Where(d => d.IsDel == false).ToList();
            
            

            return View();

        }


        /// <summary>
        /// 获取调查具体项
        /// </summary>
        /// <param name="DepID">部门ID</param>
        /// <param name="itemTypeid">类型ID</param>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ActionResult GetSurveyItemData(int DepID, int itemTypeid, int page, int limit)
        {


            List<SatisfactionSurveyView> resultlist = new List<SatisfactionSurveyView>();

            try
            {
                var list = db_survey.Screen(DepID, itemTypeid);

                foreach (var item in list)
                {

                    var viewobj = db_survey.ConvertModelView(item);

                    if (viewobj != null)
                    {
                        resultlist.Add(viewobj);
                    }
                }

            }
            catch (Exception ex)
            {

                Base_UserBusiness.WriteSysLog("查询数据出错了 位置 ：满意度调查GetSurveyData ", EnumType.LogType.加载数据);
            }

            int count = resultlist.Count;

            var obj = new {

                code = 0,
                msg = "",
                count = count,
                data = resultlist.Skip((page - 1) * limit).Take(limit).ToList()


            };

            return Json(obj, JsonRequestBehavior.AllowGet);


        }



        /// <summary>
        /// 获取调查项类型数据
        /// </summary>
        /// <param name="typename"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult GetSurveyItemTypeData(string  typename, int depid)
        {
            AjaxResult result = new AjaxResult();

            List<SatisficingType> resultlist = new List<SatisficingType>();

            try
            {
                resultlist = db_survey.Screen(typename,  depid);

                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = resultlist;

            }
            catch (Exception ex)
            {
                
                result.ErrorCode = 500;
                result.Msg = ex.Message;
                result.Data = resultlist;
            }

            return Json(result,JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 添加调查项类型
        /// </summary>
        /// <param name="typename">类名称</param>
        /// <param name="depid">部门</param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult AddSurveyItemType(string typename,int depid)
        {
            AjaxResult result = new AjaxResult();

            try
            {
                db_survey.AddSurveyItemType(typename, depid);

                result.ErrorCode = 200;
                result.Msg = "成功";
                result.Data = null;
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Msg = ex.Message;
                result.Data = null;
            }

            

            return Json(result, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public ActionResult GetGetSurveyItemTypeDataByDepid(int depid)
        {
            AjaxResult result = new AjaxResult();

            List<SatisficingType> resultlist = new List<SatisficingType>();

            try
            {

                resultlist = db_survey.Screen(null, depid);

                result.ErrorCode = 200;
                result.Data = resultlist;
                result.Msg = "成功";

            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = resultlist;
                result.Msg = ex.Message;
            }

            return Json (result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加满意度调查具体项
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AddSurveyItem()
        {
            //提供部门数据

           ViewBag.Dep = db_dep.GetList().Where(d => d.IsDel == false).ToList();

            return View();
        }

        /// <summary>
        /// 添加满意度调查具体项
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddSurveyItem(SatisficingItem satisficingItem)
        {
            AjaxResult result = new AjaxResult();

            satisficingItem.IsDel = false;

            try
            {
                db_survey.Insert(satisficingItem);

                result.ErrorCode = 200;
                result.Data = null;
                result.Msg = "成功";
            }
            catch (Exception ex)
            {

                result.ErrorCode = 500;
                result.Data = null;
                result.Msg =ex.Message;
            }


            return Json(result);

        }


    }
}