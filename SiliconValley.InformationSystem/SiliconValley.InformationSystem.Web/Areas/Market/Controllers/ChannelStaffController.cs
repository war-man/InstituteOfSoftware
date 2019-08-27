using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Channel;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.Psychro;
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
        /// 员工业务类
        /// </summary>
        private EmployeesInfoManage dbempinfo;
        /// <summary>
        /// 渠道员工业务类
        /// </summary>
        private ChannelStaffBusiness dbchastaff;
        /// <summary>
        /// 学校年度计划业务类
        /// </summary>
        private SchoolYearPlanBusiness dbschoolpaln;
        /// <summary>
        /// 渠道专员区域分布
        /// </summary>
        private EmployeeAreaBusiness dbemparea;


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
        public ActionResult ChannelStaffIndex()
        {
            dbschoolpaln = new SchoolYearPlanBusiness();
            ViewBag.YearName = dbschoolpaln.GetAll().Select(a => new 
            {
                ID = a.ID,
                YearName = a.PlanDate.Year.ToString() + "年"
            }) ;
            ViewBag.NowYearName = DateTime.Now.Year.ToString() + "年";
            return View();
        }


        /// <summary>
        /// 加载渠道员工数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetChannelStaffData(int page, int limit,string YearName)
        {
            dbchastaff = new ChannelStaffBusiness();
            dbempinfo = new EmployeesInfoManage();
            var data= dbchastaff.GetChannelsByYear(YearName);

            var newdata = data.Select(a => new 
            {
                ChannelStaffID = a.ID,
                EmpinfoID = a.EmployeesInfomation_Id,
                ChannelDate = a.ChannelDate,
                EmpName = dbempinfo.GetInfoByEmpID(a.EmployeesInfomation_Id).EmpName,
                Phone = dbempinfo.GetInfoByEmpID(a.EmployeesInfomation_Id).Phone,
              

            });
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = newdata.Count(),
                data = newdata
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 借资/预资页面
        /// </summary>
        /// <returns></returns>
        public ActionResult BorrowMoney()
        {
            return View();
        }
        /// <summary>
        /// 普通借资
        /// </summary>
        /// <param name="debit"></param>
        /// <returns></returns>
        public ActionResult PublicBorrow(Debit debit)
        {
            dbempinfo = new EmployeesInfoManage();
            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbempinfo.Borrowmoney(debit);
                BusHelper.WriteSysLog("当渠道员工普通借资的时候，位于Market区域ChannelStaffController控制器中PublicBorrow方法，添加成功。", EnumType.LogType.添加数据);
                ajaxResult.Data = "";
                ajaxResult.ErrorCode = 200;
                ajaxResult.Success = true;
                ajaxResult.Msg = "借资成功";
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("当渠道员工普通借资的时候，位于Market区域ChannelStaffController控制器中PublicBorrow方法，添加失败。", EnumType.LogType.添加数据);

            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 预资表添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Prefunding()
        {

            return null;
        }

    }
}