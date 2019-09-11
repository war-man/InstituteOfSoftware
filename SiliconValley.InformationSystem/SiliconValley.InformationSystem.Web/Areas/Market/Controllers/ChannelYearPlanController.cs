using SiliconValley.InformationSystem.Business.Channel;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.Psychro;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Entity.ViewEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Market.Controllers
{
    public class ChannelYearPlanController : Controller
    {
        /// <summary>
        /// 学校安排
        /// </summary>
        private SchoolYearPlanBusiness dbschoolpaln;
        /// <summary>
        /// 渠道员工业务类
        /// </summary>
        private ChannelStaffBusiness dbchannelstaff;
        /// <summary>
        /// 员工业务类
        /// </summary>
        private EmployeesInfoManage dbempstaff;

        /// <summary>
        /// 借资业务类
        /// </summary>
        private DebitBusiness dbdebit;
        /// <summary>
        /// 预资业务类
        /// </summary>
        private PrefundingBusiness dbfunding;
        /// <summary>
        /// 备案业务类
        /// </summary>
        private StudentDataKeepAndRecordBusiness dbbeian;

        /// <summary>
        /// 渠道负责区域业务类
        /// </summary>
        private ChannelAreaBusiness dbarea;
        /// <summary>
        /// 区域业务类
        /// </summary>
        private RegionBusiness dbregion;
        /// <summary>
        /// 异动业务类
        /// </summary>
        private MrdEmpTransactionBusiness dbyidong;
        // GET: Market/ChannelYearPlan
        public ActionResult ChannelYearPlanIndex()
        {
            dbschoolpaln = new SchoolYearPlanBusiness();
            dbchannelstaff = new ChannelStaffBusiness();
            dbempstaff = new EmployeesInfoManage();

            var data = dbschoolpaln.GetAll().OrderByDescending(a => a.ID).Select(a => new ShowyearnameView
            {
                SchoolPlanID = a.ID,
                ShowTitle = a.Title
            }).ToList();

            ViewBag.YearName = data;
            var nowyear = data.FirstOrDefault();
            ViewBag.NowYearName = nowyear;
            var nowpaln = dbschoolpaln.GetPlanByID(nowyear.SchoolPlanID);
            //加载该年的主任
            var zhurenlist = dbchannelstaff.GetChannelZhurenByPlan(nowpaln, dbschoolpaln);
            var empzhuren = new List<EmployeesInfo>();
            foreach (var item in zhurenlist)
            {
                var bubu = dbempstaff.GetInfoByEmpID(item.EmployeesInfomation_Id);
                empzhuren.Add(bubu);
            }
            ViewBag.empzhuren = empzhuren;
            return View();
        }
        /// <summary>
        /// 表格数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ChannelYearPlanData(int page, int limit, int? PlanID, string EmpID)
        {

            dbchannelstaff = new ChannelStaffBusiness();
            dbschoolpaln = new SchoolYearPlanBusiness();
            dbempstaff = new EmployeesInfoManage();
            dbbeian = new StudentDataKeepAndRecordBusiness();
            dbdebit = new DebitBusiness();
            dbfunding = new PrefundingBusiness();
            dbarea = new ChannelAreaBusiness();
            dbregion = new RegionBusiness();
            int? selectplan = 0;
            if (PlanID != null)
            {
                selectplan = PlanID;
            }
            else
            {
                var plan = dbschoolpaln.GetAll().LastOrDefault();
                selectplan = plan.ID;
            }
            //该年
            var planinfo = dbschoolpaln.GetPlanByID(selectplan);

            //获取该年有的人
            var data = dbchannelstaff.GetChannelByYear(planinfo, dbschoolpaln);

            //排序的数据
            var sortdata = new List<ChannelStaff>();
            //加载该年的主任
            var zhurenlist = dbchannelstaff.GetChannelZhurenByPlan(planinfo, dbschoolpaln);


            //删除主任
            for (int i = data.Count - 1; i >= 0; i--)
            {
                foreach (var item in zhurenlist)
                {
                    if (data[i].EmployeesInfomation_Id == item.EmployeesInfomation_Id)
                    {
                        data.Remove(data[i]);
                    }
                }
            }


            //加载有主任系列的员工
            foreach (var item in zhurenlist)
            {
                //主任以及自己的团队
                var resultzhurenteam = dbarea.GetTeamByEmpID(item.EmployeesInfomation_Id, planinfo, data);
                sortdata.AddRange(resultzhurenteam);
            }
            //删除已经sortdata集合有的员工
            for (int i = data.Count - 1; i >= 0; i--)
            {
                foreach (var item in sortdata)
                {
                    if (data[i].EmployeesInfomation_Id == item.EmployeesInfomation_Id)
                    {
                        data.Remove(data[i]);
                    }
                }
            }
            //加载还存在的data数据
            sortdata.AddRange(data);

            if (!string.IsNullOrEmpty(EmpID))
            {
                foreach (var mrdzhuren in zhurenlist)
                {
                    if (mrdzhuren.EmployeesInfomation_Id == EmpID)
                    {
                        //主任以及自己的团队
                        sortdata = dbarea.GetTeamByEmpID(EmpID, planinfo, data);
                        break;
                    }
                }
            }

            List<MrdChannelYearPlanIndexView> mrdplan = new List<MrdChannelYearPlanIndexView>();
            foreach (var item in sortdata)
            {
                //获取这个渠道员工这一年的上门量
                var goschoolcount = dbbeian.GetGoSchoolByPlan(selectplan, item.EmployeesInfomation_Id);
                //获取这个渠道员工这一年的备案量
                var beiancount = dbbeian.GetBeanCount(item.EmployeesInfomation_Id, selectplan);
                //获取这个渠道员工这一年的学员报名量
                var baomingcount = dbbeian.GetBaoMingCount(item.EmployeesInfomation_Id, selectplan);
                //获取借资量与预资量
                var debitcount = dbdebit.GetDebitsByYear(item.EmployeesInfomation_Id, planinfo);
                var fundingcoung = dbfunding.GetPrefundingsByYear(item.EmployeesInfomation_Id, planinfo);

                //获取员工该年度计划负责区域
                var channelarea = dbarea.GetAreaByPaln(item.ID, planinfo);

                var mrdRegionName = "";
                var mrRegionID = "";
                if (channelarea.Count != 0)
                {
                    foreach (var mrdarea in channelarea)
                    {
                        var region = dbregion.GetRegionByID(mrdarea.RegionID);
                        mrdRegionName = mrdRegionName == "" ? region.RegionName : mrdRegionName + "、" + region.RegionName;
                        mrRegionID = mrRegionID == "" ? region.ID.ToString() : mrRegionID + "、" + region.ID.ToString();
                    }
                }

                MrdChannelYearPlanIndexView mrdChannel = new MrdChannelYearPlanIndexView();
                var empinfo = dbempstaff.GetInfoByEmpID(item.EmployeesInfomation_Id);
                mrdChannel.ChannelDate = empinfo.EntryTime;
                mrdChannel.ChannelStaffID = item.ID;
                mrdChannel.ChannelYearPlanID = selectplan;
                mrdChannel.BeianNumber = beiancount.Count;
                mrdChannel.GoSchoolNumber = goschoolcount.Count;
                mrdChannel.EmpName = empinfo.EmpName;
                mrdChannel.Phone = empinfo.Phone;
                mrdChannel.PlanNumber = planinfo.AreaNumber;
                mrdChannel.Region = mrdRegionName;
                mrdChannel.RegionID = mrRegionID;
                mrdChannel.SignUpNumber = baomingcount.Count;
                mrdChannel.QuitDate = item.QuitDate;
                mrdChannel.EmpStaffID = item.EmployeesInfomation_Id;
                mrdChannel.DebitNumber = debitcount.Count + fundingcoung.Count;
                mrdplan.Add(mrdChannel);
            }


            var bnewdata = mrdplan.Skip((page - 1) * limit).Take(limit).ToList();
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = mrdplan.Count(),
                data = bnewdata
            };
            return Json(returnObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewChannelYearPlanIndex()
        {
            dbschoolpaln = new SchoolYearPlanBusiness();
            dbchannelstaff = new ChannelStaffBusiness();
            dbempstaff = new EmployeesInfoManage();

            var data = dbschoolpaln.GetAll().OrderByDescending(a => a.ID).Select(a => new ShowyearnameView
            {
                SchoolPlanID = a.ID,
                ShowTitle = a.Title
            }).ToList();

            ViewBag.YearName = data;
            var nowyear = data.FirstOrDefault();
            ViewBag.NowYearName = nowyear;
            var nowpaln = dbschoolpaln.GetPlanByID(nowyear.SchoolPlanID);
            //加载该年的主任
            var zhurenlist = dbchannelstaff.GetChannelZhurenByPlan(nowpaln, dbschoolpaln);
            var empzhuren = new List<EmployeesInfo>();
            foreach (var item in zhurenlist)
            {
                var bubu = dbempstaff.GetInfoByEmpID(item.EmployeesInfomation_Id);
                empzhuren.Add(bubu);
            }
            ViewBag.empzhuren = empzhuren;
          
            return View();
        }
        /// <summary>
        /// 加载主任下拉框
        /// </summary>
        /// <param name="Plan"></param>
        /// <returns></returns>
        public ActionResult LoadZhuren(int Plan)
        {
            dbschoolpaln = new SchoolYearPlanBusiness();
            dbchannelstaff = new ChannelStaffBusiness();
            dbempstaff = new EmployeesInfoManage();
            var planinfo = dbschoolpaln.GetPlanByID(Plan);
            //加载该年的主任
            var zhurenlist = dbchannelstaff.GetChannelZhurenByPlan(planinfo, dbschoolpaln);
            var empzhuren = new List<EmployeesInfo>();
            foreach (var item in zhurenlist)
            {
                var bubu = dbempstaff.GetInfoByEmpID(item.EmployeesInfomation_Id);
                empzhuren.Add(bubu);
            }
            return Json(empzhuren, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据修改年度下拉框加载树形列表
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadTree_Zhuren(int Plan, string zhurenid)
        {

            return Json(this.MyLoad(Plan,zhurenid),JsonRequestBehavior.AllowGet);
        }

        private List<TreeClass> MyLoad(int Plan,string zhurenid)
        {
            dbschoolpaln = new SchoolYearPlanBusiness();
            dbchannelstaff = new ChannelStaffBusiness();
            dbempstaff = new EmployeesInfoManage();
            //该年
            var planinfo = dbschoolpaln.GetPlanByID(Plan);
            //获取该年有的人
            var data = dbchannelstaff.GetChannelByYear(planinfo, dbschoolpaln);

            //加载该年的主任
            List<TreeClass> zhurenlist = new List<TreeClass>();

            //没传id过来就是说明是全部主任，如果传过来id就是加载这个主任下面的员工
            if (string.IsNullOrEmpty(zhurenid))
            {

                zhurenlist = dbchannelstaff.GetChannelZhurenByPlan(planinfo, dbschoolpaln).Select(a => new TreeClass()
                {
                    title = dbempstaff.GetInfoByEmpID(a.EmployeesInfomation_Id).EmpName,
                    id = a.EmployeesInfomation_Id,
                    spread=true
                }).ToList();
            }
            else
            {
              var empinfo= dbempstaff.GetInfoByEmpID(zhurenid);
                TreeClass  treeClass= new TreeClass();
                treeClass.title = empinfo.EmpName;
                treeClass.id = empinfo.EmployeeId;
                treeClass.spread = true;
                zhurenlist.Add(treeClass);
            }

            foreach (var item in zhurenlist)
            {
                List<TreeClass> fuzhuren = dbchannelstaff.GetFUzhurenByPlan(item.id, planinfo, data).Select(a => new TreeClass()
                {
                    title = a.EmpName,
                    id = a.EmployeeId,
                    spread = true
                }).ToList();

                foreach (var mrditem in fuzhuren)
                {
                    List<TreeClass> empinfo = dbchannelstaff.GetFUzhurenByPlan(mrditem.id, planinfo, data).Select(a => new TreeClass()
                    {
                        title = a.EmpName,
                        id = a.EmployeeId,
                        spread = true
                    }).ToList();
                    mrditem.children = empinfo;
                }
                item.children = fuzhuren;
            }

            return zhurenlist;
        }

        /// <summary>
        /// 加载图
        /// </summary>
        /// <param name="EmpinfoID"></param>
        /// <param name="IsTeamint"></param>
        /// <param name="planid"></param>
        /// <returns></returns>
        public ActionResult LoadEcharts(string EmpinfoID,bool IsTeam, int planid) {
            dbschoolpaln = new SchoolYearPlanBusiness();
            dbchannelstaff = new ChannelStaffBusiness();
            dbarea = new ChannelAreaBusiness();
            dbempstaff = new EmployeesInfoManage();
            var nowplan = dbschoolpaln.GetPlanByID(planid);
            //这是该年工作的员工
            var data = dbchannelstaff.GetChannelByYear(nowplan, dbschoolpaln);
            //如果是一个主任团队 那就肯定主任的员工编号
            if (IsTeam)
            {
                //判断有没有员工编号
                if (!string.IsNullOrEmpty(EmpinfoID))
                {
                    var zhurenTeam = dbarea.GetTeamByEmpID(EmpinfoID, nowplan, data);
                    var result = this.ResultECharts(zhurenTeam, planid);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = this.ResultECharts(data, planid);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                   
            }
            
            //点击的是杨校就是显示全年的总体的效果或者是点击的是个人体现的是个人业绩
            else
            {
                //判断有没有员工编号
                if (!string.IsNullOrEmpty(EmpinfoID))
                {
                    //这个员工编号需要判断是不是杨校的员工编号
                    var yangxiao =  dbempstaff.GetYangxiao();
                    //展示全年的整体的数据
                    if (yangxiao.EmployeeId==EmpinfoID)
                    {
                        var result = this.ResultECharts(data, planid);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    //展示当前这个员工的这一年的业绩
                    else
                    {
                        List<ChannelStaff> mydata = new List<ChannelStaff>();
                        var channelstaff= dbchannelstaff.GetChannelByEmpID(EmpinfoID);
                        mydata.Add(channelstaff);
                        var result = this.ResultECharts(mydata, planid);
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    
                }
                //显示全部
                else
                {
                    var result = this.ResultECharts(data, planid);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            
            
        }

        /// <summary>
        /// 合并方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<ShowEchartsView> ResultECharts(List<ChannelStaff> data, int planid) {
            dbbeian = new StudentDataKeepAndRecordBusiness();
            dbchannelstaff = new ChannelStaffBusiness();
            List<ShowEchartsView> result = new List<ShowEchartsView>();
            for (int i = 1; i <= 12; i++)
            {
                ShowEchartsView show = new ShowEchartsView();
                int mrdKeepOnRecordCount = 0;
                int mrdGoSchoolCount = 0;
                int mrdSignUpCount = 0;
                foreach (var item in data)
                {
                    var list = dbbeian.GetBeanCount(item.EmployeesInfomation_Id, planid);
                    var queryKeepOnRecordCount = dbchannelstaff.GetChannelMonthKeepOnRecord(i, list).Count;
                    var queryGoSchoolCount = dbchannelstaff.GetChannelGoSchoolCount(i, list).Count;
                    var querSignUpCount = dbchannelstaff.GetChannelSignUpCount(i, list).Count;
                    mrdKeepOnRecordCount += queryKeepOnRecordCount;
                    mrdGoSchoolCount += queryGoSchoolCount;
                    mrdSignUpCount += querSignUpCount;
                }
                show.KeepOnRecordCount = mrdGoSchoolCount;
                show.GoSchoolCount = mrdGoSchoolCount;
                show.SignUpCount = mrdSignUpCount;
                result.Add(show);
            }
            return result;
        }
    }
}