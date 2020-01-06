using Newtonsoft.Json.Linq;
using SiliconValley.InformationSystem.Business.Base_SysManage;
using SiliconValley.InformationSystem.Business.Channel;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Business.Psychro;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity.MarketView;
using SiliconValley.InformationSystem.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    [CheckLogin]
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
        /// 备案业务类
        /// </summary>
        private StudentDataKeepAndRecordBusiness dbbeian;
        /// <summary>
        /// 就业专员业务类
        /// </summary>
        private EmploymentStaffBusiness dbempstaff;
        /// <summary>
        /// 预资业务类
        /// </summary>
        private PrefundingBusiness dbprefunding;
        /// <summary>
        /// 预资详细
        /// </summary>
        private PerInfoBusiness dbperinfo;
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
                //现在
                var channelarea = dbemparea.GetAreasingByChannelID(item.ID);
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

            dbempinfo = new EmployeesInfoManage();
            dbchastaff = new ChannelStaffBusiness();

            //全部数据
            var data = dbregion.GetRegions();
            var yangxiao = new EmployeesInfo();
            //获取分配的数据
            var yes = dbregion.GetRegionsByempid(id);

            var empinfo = dbempinfo.GetInfoByChannelID(id);
     
            List<EmployeesInfo> myempinfolist = new List<EmployeesInfo>();

            if (dbempinfo.IsFuzhiren(empinfo))
            {
                //拿主任列表
                myempinfolist = dbempinfo.GetChannelStaffZhuren();

            }
            else if (dbempinfo.IsChannelZhuren(empinfo))
            {
                yangxiao = dbempinfo.GetYangxiao();
                myempinfolist.Add(yangxiao);
            }
            else
            {
                //拿副主任列表
                myempinfolist = dbempinfo.GetChannelStaffFuzhuren();
            }
            List<DistributionAreaView> result = new List<DistributionAreaView>();
            foreach (var item in myempinfolist)
            {

                DistributionAreaView areaView = new DistributionAreaView();
                areaView.Text = item.EmpName;
                if (yangxiao.EmployeeId!=null)
                {
                    areaView.Value = null;
                }
                else
                {
                    areaView.Value = dbchastaff.GetChannelByEmpID(item.EmployeeId).ID;
                }
                result.Add(areaView);
            }
          
            ViewBag.regions = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            ViewBag.shangji = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            ViewBag.yes = Newtonsoft.Json.JsonConvert.SerializeObject(yes);
            return View();
        }
        [HttpPost]
        /// <summary>
        /// 执行分配
        /// </summary>
        /// <returns></returns>
        public ActionResult DoDistribution(DistributionView param0)
        {

            AjaxResult ajaxResult = new AjaxResult();
            try
            {
                dbemparea = new ChannelAreaBusiness();
                dbchastaff = new ChannelStaffBusiness();
                dbempinfo = new EmployeesInfoManage();

                ///如果没有数据，将会把这个员工区域全部删除
                ///将已经存在的数据进行一个排除，将这个regions 集合中存在的数据进行一个删除，
                ///判断以前的区域主管是否是当前传入过来的主管id 如果不是进行一个修改数据
                ///进行添加操作
                if (string.IsNullOrEmpty(param0.RegionIDs))
                {
                    List<ChannelArea> areas = dbemparea.GetAreaByChannelID(param0.ChannelStaffID);
                    //删除存在数据中现在没有的数据
                    foreach (var item in areas)
                    {
                        item.IsDel = true;
                        dbemparea.Update(item);
                    }
                }
                else
                {
                    List<string> regions = param0.RegionIDs.Split(',').ToList();
                    List<ChannelArea> areas = dbemparea.GetAreaByChannelID(param0.ChannelStaffID);
                    for (int i = regions.Count - 1; i >= 0; i--)
                    {
                        for (int j = areas.Count - 1; j >= 0; j--)
                        {
                            //排除相同的。
                            if (int.Parse(regions[i]) == areas[j].RegionID)
                            {
                                regions.RemoveAt(i);
                                areas.RemoveAt(j);
                                break;
                            }
                        }
                    }

                    //删除存在数据中现在没有的数据
                    foreach (var item in areas)
                    {
                        item.IsDel = true;
                        dbemparea.Update(item);
                    }



                    ///拿取删除过后的数据
                    List<ChannelArea> newareas = dbemparea.GetAreaByChannelID(param0.ChannelStaffID);
                    foreach (var item in newareas)
                    {
                        if (param0.RegionalDirectorID != item.RegionalDirectorID)
                        {
                            item.RegionalDirectorID = param0.RegionalDirectorID;
                            dbemparea.Update(item);
                        }
                    }

                    ///添加
                    foreach (var item in regions)
                    {
                        ChannelArea channelArea = new ChannelArea();
                        channelArea.ChannelStaffID = param0.ChannelStaffID;
                        channelArea.IsDel = false;
                        channelArea.RegionalDirectorID = param0.RegionalDirectorID;
                        channelArea.RegionID = int.Parse(item);
                        channelArea.StaffAreaDate = DateTime.Now;
                        dbemparea.Insert(channelArea);

                    }
                }
               

                ajaxResult.Success = true;
            }
            catch (Exception ex)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请及时联系信息部成员。";
            }
           
        
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }




    }
}