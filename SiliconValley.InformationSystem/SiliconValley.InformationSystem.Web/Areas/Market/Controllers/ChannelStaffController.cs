using Newtonsoft.Json.Linq;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Channel;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.Psychro;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
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
        private ChannelAreaBusiness dbemparea;
        /// <summary>
        /// 区域业务类
        /// </summary>
        private RegionBusiness dbregion;
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
            return View();
        }

        /// <summary>
        /// 加载渠道员工数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetChannelStaffData(int page, int limit)
        {
            dbchastaff = new ChannelStaffBusiness();
            dbempinfo = new EmployeesInfoManage();
            dbemparea = new ChannelAreaBusiness();
            dbregion = new RegionBusiness();
            var data = dbchastaff.GetChannelStaffs();
            List<ChannelStaffIndexView> resultlist = new List<ChannelStaffIndexView>();
            foreach (var item in data)
            {
                var empinfo = dbempinfo.GetInfoByEmpID(item.EmployeesInfomation_Id);
                ChannelStaffIndexView indexView = new ChannelStaffIndexView();
                indexView.ChannelStaffID = item.ID;
                indexView.EmployeeId = item.EmployeesInfomation_Id;
                indexView.EmpName = empinfo.EmpName;
                indexView.EntryTime = empinfo.EntryTime;
                indexView.Phone = empinfo.Phone;
                indexView.PositiveDate = empinfo.PositiveDate;
                indexView.Remark = empinfo.Remark;
                //拿主任列表
                var zhuren = dbempinfo.GetChannelStaffZhuren();

                bool iszhuren = false;
                foreach (var foritem in zhuren)
                {
                    if (foritem.EmployeeId == item.EmployeesInfomation_Id)
                        iszhuren = true;
                }

                var channelarea = dbemparea.GetAreaByChannelID(item.ID);
                if (channelarea.Count != 0)
                {
                    var mrdRegionName = "";
                    var mrRegionID = "";
                    foreach (var mrdarea in channelarea)
                    {
                        var region = dbregion.GetRegionByID(mrdarea.RegionID);
                        mrdRegionName = mrdRegionName == "" ? region.RegionName : mrdRegionName + "、" + region.RegionName;
                        mrRegionID = mrRegionID == "" ? region.ID.ToString() : mrRegionID + "、" + region.ID.ToString();
                        if (iszhuren)
                        {
                            var yangxiao = dbempinfo.GetYangxiao();
                            indexView.RegionalDirectorEmpName = yangxiao.EmpName;
                            indexView.RegionalDirectorID = null;
                        }
                        else
                        {
                            var zhugaun = dbchastaff.GetChannelByID(mrdarea.RegionalDirectorID);
                            var zhugauninfo = dbempinfo.GetInfoByEmpID(zhugaun.EmployeesInfomation_Id);
                            var channelinfo = dbchastaff.GetChannelByEmpID(zhugauninfo.EmployeeId);
                            indexView.RegionalDirectorEmpName = zhugauninfo.EmpName;
                            indexView.RegionalDirectorID = channelinfo.ID;
                        }
                        indexView.RegionName = mrdRegionName;
                        indexView.RegionID = mrRegionID;
                    }
                }
                else
                {
                    indexView.RegionalDirectorEmpName = "";
                    indexView.RegionName = "";
                    indexView.RegionalDirectorID = null;
                    indexView.RegionID = "";

                }
                resultlist.Add(indexView);

            }
            var bnewdata = resultlist.Skip((page - 1) * limit).Take(limit).ToList();
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = resultlist.Count(),
                data = bnewdata
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        /// <summary>
        /// get 请求分配区域
        /// </summary>
        /// <returns></returns>
        public ActionResult DistributionArea(int id)
        {

            ViewBag.ChannelStaffID = id;
            dbregion = new RegionBusiness();
            dbemparea = new ChannelAreaBusiness();
            dbempinfo = new EmployeesInfoManage();
            dbchastaff = new ChannelStaffBusiness();
            //拿没有分配的区域
            var data = dbregion.GetNoDistribution(dbemparea);
            //拿主任列表
            var zhuren = dbempinfo.GetChannelStaffZhuren();
            var channelstaff = dbchastaff.GetChannelByID(id);
            bool iszhuren = false;
            foreach (var item in zhuren)
            {
                if (item.EmployeeId == channelstaff.EmployeesInfomation_Id)
                {
                    iszhuren = true;
                }
            }
            if (iszhuren)
            {
                List<EmployeesInfo> myempinfolist = new List<EmployeesInfo>();
                var yangxiao = dbempinfo.GetYangxiao();
                myempinfolist.Add(yangxiao);
                ViewBag.shangji = myempinfolist.Select(a => new SelectListItem
                {
                    Text = a.EmpName,
                    Value = a.EmployeeId.ToString()
                }).ToList();
            }
            else
            {
                ViewBag.shangji = zhuren.Select(a => new SelectListItem
                {
                    Text = a.EmpName,
                    Value = a.EmployeeId.ToString()
                }).ToList();
            }
            ViewBag.regions = Newtonsoft.Json.JsonConvert.SerializeObject(data);



            return View();
        }
        [HttpPost]
        /// <summary>
        /// 执行分配
        /// </summary>
        /// <returns></returns>
        public ActionResult DoDistribution(string empinfoid, int channelstaffid, string jsonarr)
        {
            dbemparea = new ChannelAreaBusiness();
            dbchastaff = new ChannelStaffBusiness();
            dbempinfo = new EmployeesInfoManage();
            JArray jArray = JArray.Parse(jsonarr);
            AjaxResult ajaxResult = new AjaxResult();
            var mychannelstaff = dbchastaff.GetChannelByID(channelstaffid);
            bool iszhuren = dbempinfo.IsChannelZhuren(mychannelstaff.EmployeesInfomation_Id);
            foreach (var item in jArray)
            {
                JObject jdata = (JObject)item;
                ChannelArea channelArea = new ChannelArea();
                channelArea.ChannelStaffID = channelstaffid;
                if (iszhuren)
                {
                    channelArea.RegionalDirectorID = null;
                }
                else
                {
                    var empinfo = dbchastaff.GetChannelByEmpID(empinfoid);
                    channelArea.RegionalDirectorID = empinfo.ID;
                }
                channelArea.RegionID = int.Parse(jdata["value"].ToString());
                channelArea.IsDel = false;
                channelArea.StaffAreaDate = DateTime.Now;
                try
                {
                    dbemparea.Insert(channelArea);
                    BusHelper.WriteSysLog("给渠道员工分配区域的时候，位于Market区域ChannelStaffController控制器中DoDistribution方法，添加成功。", EnumType.LogType.添加数据);
                    ajaxResult = dbemparea.Success("分配成功");
                }
                catch (Exception ex)
                {

                    BusHelper.WriteSysLog("给渠道员工分配区域的时候，位于Market区域ChannelStaffController控制器中DoDistribution方法，添加失败。" + ex.Message, EnumType.LogType.添加数据);
                    ajaxResult = dbemparea.Error("分配失败");
                }

            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
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