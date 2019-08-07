using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Channel;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class ChannelStaffController : Controller
    {
        /// <summary>
        /// 进入市场渠道页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ChannelStaffBusiness channelStaffBusiness = new ChannelStaffBusiness();
            var aa = channelStaffBusiness.GetList();
            return View();
        }

        /// <summary>
        /// 显示市场渠道员工
        /// </summary>
        /// <returns></returns>
        public ActionResult ChannelStaffList() {
            return View();
        }

        /// <summary>
        /// 加载渠道员工数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetChannelStaffData()
        {
            //获取登陆用户的信息
            Base_User base_User = SessionHelper.Session["UserId"] as Base_User;
            //通过登陆用户找到员工


            //CocStrategyEntities cocdbcontent = new CocStrategyEntities();
            //var data = cocdbcontent.ArmsInfo.Where(a => a.IsDel == false).ToList();
            //var newdata = data.Select(a => new Arms
            //{
            //    ID = a.ID,
            //    ArmsName = a.ArmsName,
            //    Company = a.Company,
            //    Remark = a.Remark
            //});
            //var returnObj = new
            //{
            //    code = 0,
            //    msg = "",
            //    count = newdata.Count(),
            //    data = newdata
            //};
            // return Json(returnObj, JsonRequestBehavior.AllowGet);
            return null;
        }
    }
}