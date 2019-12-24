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

                ///将已经存在的数据进行一个排除，将这个regions 集合中存在的数据进行一个删除，
                ///判断以前的区域主管是否是当前传入过来的主管id 如果不是进行一个修改数据
                ///进行添加操作
                List<string> regions = param0.RegionIDs.Split(',').ToList();
                List<ChannelArea> areas= dbemparea.GetAreaByChannelID(param0.ChannelStaffID);
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

                ajaxResult.Success = true;
            }
            catch (Exception ex)
            {
                ajaxResult.Success = false;
                ajaxResult.Msg = "请及时联系信息部成员。";
            }
           
        
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 借资/预资页面
        /// </summary>
        /// <returns></returns>
        public ActionResult BorrowMoney()
        {
            //现在采用登陆员工为渠道的员工 
            //存储到session里面的id找到用户对象，用户对象就会有一个属性为员工编号。

            //现在给一个固定数据id为039e3e3891eea-1e885eb2-75a3-4f82-bb9a-570d717f2af4 员工编号为201908160008 姓名：徐宝平 部门：渠道 岗位：主任

            dbempstaff = new EmploymentStaffBusiness();

            Base_UserModel user = Base_UserBusiness.GetCurrentUser();

            //拿员工对象
            EmployeesInfo employeesInfo = dbempstaff.GetEmployeesInfoByID(user.EmpNumber);

            //拿岗位对象 
            var PositionInfo = dbempstaff.GetPositionByID(employeesInfo.PositionId);

            //拿部门对象
            var DepInfo = dbempstaff.GetDepartmentByID(PositionInfo.DeptId);

            ViewBag.PositionInfo = PositionInfo;
            ViewBag.DepInfo = DepInfo;
            ViewBag.employeesInfo = employeesInfo;
            dbbeian = new StudentDataKeepAndRecordBusiness();
            //获取的是他这个备案而且是已经报名的学生集合
            List<StudentPutOnRecord> mystudentlist = dbbeian.GetrReport(user.EmpNumber);
            //获取这个员工的预资所有的预资单
            dbprefunding = new PrefundingBusiness();
            dbperinfo = new PerInfoBusiness();
            var mydata = dbprefunding.GetAll();
            List<PerInfo> mrdperInfos = new List<PerInfo>();
            //根据这个员工的预资单获取他用过的备案编号
            foreach (var item in mydata)
            {
                List<PerInfo> resultdata = dbperinfo.GetPrefundingByID(item.ID);
                mrdperInfos.AddRange(resultdata);
            }
            //排除用过的备案编号
            for (int i = mystudentlist.Count - 1; i >= 0; i--)
            {
                foreach (var item in mrdperInfos)
                {
                    if (mystudentlist[i].Id == item.BeianID)
                    {
                        mystudentlist.Remove(mystudentlist[i]);
                    }
                }
            }
            //存放的是这个员工备案数据已经报名的学生但未用这个学生借资过的。
            ViewBag.Student = Newtonsoft.Json.JsonConvert.SerializeObject(mystudentlist);
            return View();
        }
        /// <summary>
        /// 普通借资
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        public ActionResult PublicBorrow(string EmployeeId, float DebitMoney, string BorrowmoneyReason)
        {
            dbempinfo = new EmployeesInfoManage();
            AjaxResult ajaxResult = new AjaxResult();
            Debit debit = new Debit();
            debit.date = DateTime.Now;
            debit.DebitMoney = DebitMoney;
            debit.Debitwhy = BorrowmoneyReason;
            debit.EmpNumber = EmployeeId;
            debit.IsDel = false;
            debit.Remark = string.Empty;
            try
            {
                dbempinfo.Borrowmoney(debit);
                BusHelper.WriteSysLog("当员工普通借资的时候，位于Market区域ChannelStaffController控制器中PublicBorrow方法，添加成功。", EnumType.LogType.添加数据);
                ajaxResult.Data = "";
                ajaxResult.ErrorCode = 200;
                ajaxResult.Success = true;
                ajaxResult.Msg = "借资成功";
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("当员工普通借资的时候，位于Market区域ChannelStaffController控制器中PublicBorrow方法，添加失败。", EnumType.LogType.添加数据);

            }
            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 预资表添加
        /// </summary>
        /// <returns></returns>
        public ActionResult DoPrefunding(string EmployeeId, float DebitMoney, string transferdata)
        {
            dbprefunding = new PrefundingBusiness();

            dbperinfo = new PerInfoBusiness();
            AjaxResult ajaxResult = new AjaxResult();
            JArray jArray = JArray.Parse(transferdata);
            Prefunding myPrefunding = new Prefunding();
            myPrefunding.EmpNumber = EmployeeId;
            myPrefunding.IsDel = false;
            myPrefunding.PerMoney = DebitMoney;

            var datetime = DateTime.Now;
            var baiozhi = datetime.ToFileTimeUtc().ToString();
            myPrefunding.PreDate = datetime;
            myPrefunding.Remark = baiozhi;
            try
            {
                dbprefunding.Insert(myPrefunding);
                BusHelper.WriteSysLog("当员工预资的时候，位于Market区域ChannelStaffController控制器中DoPrefunding方法，添加成功。", EnumType.LogType.添加数据);
                ajaxResult = dbprefunding.Success("添加成功");
                dbprefunding = new PrefundingBusiness();
                var mydata = dbprefunding.GetAll();
                var dudu = mydata.Where(a => a.Remark == baiozhi).FirstOrDefault();
                foreach (var item in jArray)
                {
                    JObject jdata = (JObject)item;
                    PerInfo perInfo = new PerInfo();
                    perInfo.IsDel = false;
                    perInfo.PreID = dudu.ID;
                    perInfo.Remark = string.Empty;
                    perInfo.BeianID = int.Parse(jdata["value"].ToString());
                    try
                    {
                        dbperinfo.Insert(perInfo);
                        BusHelper.WriteSysLog("当员工预资详细表的时候，位于Market区域ChannelStaffController控制器中DoPrefunding方法，添加成功。", EnumType.LogType.添加数据);
                        ajaxResult = dbperinfo.Success("添加成功");
                    }
                    catch (Exception)
                    {

                        BusHelper.WriteSysLog("当员工预资详细表的时候，位于Market区域ChannelStaffController控制器中DoPrefunding方法，添加失败。", EnumType.LogType.添加数据);
                        ajaxResult = dbperinfo.Error("添加失败");
                    }
                }
            }
            catch (Exception ex)
            {
                BusHelper.WriteSysLog("当员工预资的时候，位于Market区域ChannelStaffController控制器中DoPrefunding方法，添加失败。", EnumType.LogType.添加数据);
                ajaxResult = dbprefunding.Error("添加失败");
            }

            return Json(ajaxResult, JsonRequestBehavior.AllowGet);
        }



    }
}