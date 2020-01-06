using SiliconValley.InformationSystem.Business;
using SiliconValley.InformationSystem.Business.Channel;
using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.Psychro;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
using SiliconValley.InformationSystem.Entity.Entity;
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
    //[CheckLogin]
    /// <summary>
    /// 市场年度总结
    /// </summary>
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

        private TeacherClassBusiness dbteacher;

        private HeadmasterBusiness dbheadermaster;

        private StudentInformationBusiness dbstudent;

        private ScheduleForTraineesBusiness dbclass;

        private TeacherClassBusiness dbteacherClass;


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

            ViewBag.yangxiao = Newtonsoft.Json.JsonConvert.SerializeObject(dbempstaff.GetYangxiao());
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

            return Json(this.MyLoad(Plan, zhurenid), JsonRequestBehavior.AllowGet);
        }

        private List<TreeClass> MyLoad(int Plan, string zhurenid)
        {
            dbschoolpaln = new SchoolYearPlanBusiness();
            dbchannelstaff = new ChannelStaffBusiness();
            dbempstaff = new EmployeesInfoManage();
            dbarea = new ChannelAreaBusiness();
            //该年
            var planinfo = dbschoolpaln.GetPlanByID(Plan);
            //获取该年有的人
            var data = dbchannelstaff.GetChannelByYear(planinfo, dbschoolpaln);

            //加载该年的主任
            List<TreeClass> zhurenlist = new List<TreeClass>();

            //该年的主任列表
            var querylist = dbchannelstaff.GetChannelZhurenByPlan(planinfo, dbschoolpaln);

            //传入的值是否是主任
            bool IsZhuren = false;
            //没传id过来就是说明是全部主任，如果传过来id就是加载这个主任下面的员工
            if (string.IsNullOrEmpty(zhurenid))
            {

                zhurenlist = this.allchannel(querylist);
            }
            else
            {
                //判断是不是主任
                IsZhuren = dbchannelstaff.IsZhuren(zhurenid, querylist);

                //是主任  右侧下拉框要选中这个主任
                if (IsZhuren)
                {
                    var empinfo = dbempstaff.GetInfoByEmpID(zhurenid);
                    var treeClass = this.zhurenchannel(empinfo);
                    zhurenlist.Add(treeClass);
                }
                else
                {
                    //是不是杨校 如果是杨校，加载全部员工 右侧下拉框的值不选中任何值
                    var yang = dbempstaff.GetYangxiao();
                    if (zhurenid == yang.EmployeeId)
                    {
                        zhurenlist = this.allchannel(querylist);
                    }
                    //就是普通ing员工或者是副主任，就需要加载主任级别的所有员工以及右侧下拉框要选中当前点击的这个员工的主任
                    else
                    {
                        var querychannel = dbchannelstaff.GetChannelByEmpID(zhurenid);

                        var planarealist = dbarea.GetAreasByPlan(planinfo, dbschoolpaln);
                        var quyerarea = planarealist.Where(a => a.ChannelStaffID == querychannel.ID).FirstOrDefault();
                        if (quyerarea == null)
                        {
                            zhurenlist = this.allchannel(querylist);
                        }
                        else
                        {
                            ChannelArea reusltzhuren = this.forquerychannelarea(planarealist, quyerarea);
                            var channelinfo = dbchannelstaff.GetChannelByID(reusltzhuren.ChannelStaffID);
                            var resultempinfo = dbempstaff.GetInfoByEmpID(channelinfo.EmployeesInfomation_Id);
                            var treeClass = this.zhurenchannel(resultempinfo);
                            zhurenlist.Add(treeClass);
                        }
                    }
                }

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

        private ChannelArea forquerychannelarea(List<ChannelArea> channelAreas, ChannelArea channelArea)
        {

            ChannelArea result = new ChannelArea();

            if (channelArea.RegionalDirectorID == null)
            {
                return channelArea;
            }

            foreach (var item in channelAreas)
            {

                if (channelArea.RegionalDirectorID == item.ChannelStaffID && item.RegionalDirectorID == null)
                {
                    return item;
                }

                if (channelArea.RegionalDirectorID == item.ChannelStaffID && item.RegionalDirectorID != null)
                {
                    result = forquerychannelarea(channelAreas, item);

                }


            }
            return result;




        }
        /// <summary>
        /// 全体主任
        /// </summary>
        /// <param name="querylist"></param>
        /// <returns></returns>
        private List<TreeClass> allchannel(List<ChannelStaff> querylist)
        {

            return querylist.Select(a => new TreeClass()
            {
                title = dbempstaff.GetInfoByEmpID(a.EmployeesInfomation_Id).EmpName,
                id = a.EmployeesInfomation_Id,
                spread = true
            }).ToList();

        }

        /// <summary>
        /// 单独主任
        /// </summary>
        /// <param name="empinfo"></param>
        /// <returns></returns>
        private TreeClass zhurenchannel(EmployeesInfo empinfo)
        {
            TreeClass treeClass = new TreeClass();
            treeClass.title = empinfo.EmpName;
            treeClass.id = empinfo.EmployeeId;
            treeClass.spread = true;
            return treeClass;
        }
        /// <summary>
        /// 加载图
        /// </summary>
        /// <param name="EmpinfoID"></param>
        /// <param name="IsTeamint"></param>
        /// <param name="planid"></param>
        /// <returns></returns>
        public ActionResult LoadEcharts(string EmpinfoID, bool IsTeam, int planid)
        {
            dbschoolpaln = new SchoolYearPlanBusiness();
            var nowplan = dbschoolpaln.GetPlanByID(planid);
            ChannelReportFormView result = new ChannelReportFormView();
            priLoadData(EmpinfoID, IsTeam, planid);
            var querydata = SessionHelper.Session["resultbardata"] as List<ShowEchartsView>;
            result.ShowEchartsViewData = querydata;
            result.plan = nowplan;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="EmpinfoID"></param>
        /// <param name="IsTeam"></param>
        /// <param name="planid"></param>
        /// <returns></returns>
        private void priLoadData(string EmpinfoID, bool IsTeam, int planid)
        {
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
                    SessionHelper.Session["resultbardata"] = this.ResultECharts(zhurenTeam, planid);

                }
                else
                {
                    SessionHelper.Session["resultbardata"] = this.ResultECharts(data, planid);
                }

            }
            //点击的是杨校就是显示全年的总体的效果或者是点击的是个人体现的是个人业绩
            else
            {
                //判断有没有员工编号
                if (!string.IsNullOrEmpty(EmpinfoID))
                {
                    //这个员工编号需要判断是不是杨校的员工编号
                    var yangxiao = dbempstaff.GetYangxiao();
                    //展示全年的整体的数据
                    if (yangxiao.EmployeeId == EmpinfoID)
                    {
                        SessionHelper.Session["resultbardata"] = this.ResultECharts(data, planid);
                    }
                    //展示当前这个员工的这一年的业绩
                    else
                    {
                        List<ChannelStaff> mydata = new List<ChannelStaff>();
                        var channelstaff = dbchannelstaff.GetChannelByEmpID(EmpinfoID);
                        mydata.Add(channelstaff);
                        SessionHelper.Session["resultbardata"] = this.ResultECharts(mydata, planid);
                    }

                }
                //显示全部
                else
                {
                    SessionHelper.Session["resultbardata"] = this.ResultECharts(data, planid);
                }

            }
        }
        /// <summary>
        /// 合并方法
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private List<ShowEchartsView> ResultECharts(List<ChannelStaff> data, int planid)
        {
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
                    mrdKeepOnRecordCount = mrdKeepOnRecordCount + queryKeepOnRecordCount;
                    mrdGoSchoolCount = mrdGoSchoolCount + queryGoSchoolCount;
                    mrdSignUpCount = mrdSignUpCount + querSignUpCount;
                }
                show.KeepOnRecordCount = mrdKeepOnRecordCount;
                show.GoSchoolCount = mrdGoSchoolCount;
                show.SignUpCount = mrdSignUpCount;
                result.Add(show);
            }
            return result;
        }

        /// <summary>
        /// 用于加载下面的圈圈和右边的详细
        /// </summary>
        /// <param name="EmpinfoID"></param>
        /// <param name="IsTeam"></param>
        /// <param name="planid"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public ActionResult LoadRightAndBottom(string EmpinfoID, bool IsTeam, int planid, string month)
        {

            dbschoolpaln = new SchoolYearPlanBusiness();
            dbchannelstaff = new ChannelStaffBusiness();
            dbarea = new ChannelAreaBusiness();
            dbempstaff = new EmployeesInfoManage();
            dbbeian = new StudentDataKeepAndRecordBusiness();
            dbregion = new RegionBusiness();
            var nowplan = dbschoolpaln.GetPlanByID(planid);

            MrdCircleRightView resultview = new MrdCircleRightView();
            MrdCircleView circleView = new MrdCircleView();
            MrdRightView rightView = new MrdRightView();

            //根据选择的的条件获取对应的员工
            List<ChannelStaff> Querylist = this.Resultbytiaojian(EmpinfoID, IsTeam, nowplan);

            //获取上一个年度计划
            var ThePreviousPlan = dbschoolpaln.GetThePreviousPlan(nowplan);

            int thepreviousbeiancount = 0;
            int thepreviousgoshcoolcount = 0;
            int thepreviousbaomingcount = 0;

            int nowbeiancount = 0;
            int nowgoshcoolcount = 0;
            int nowbaomingcount = 0;

            //如果是团队 预计人数就是团队人数的区域数量*区域分配量
            if (IsTeam)
            {
              
                if (!string.IsNullOrEmpty(EmpinfoID))
                {
                    var arealist = dbarea.GetAreasByPlan(nowplan, dbschoolpaln);
                    rightView.MarketForecast = arealist.Count * nowplan.AreaNumber;
                    rightView.captainname = dbempstaff.GetInfoByEmpID(EmpinfoID).EmpName;
                    rightView.teamsize = Querylist.Count;

                }
                else
                {
                    rightView.MarketForecast = nowplan.MarketForecast;
                }

            }
           
            else
            {
                //判断有没有员工编号
                if (!string.IsNullOrEmpty(EmpinfoID))
                {
                    //这个员工编号需要判断是不是杨校的员工编号
                    var yangxiao = dbempstaff.GetYangxiao();
                   
                    if (yangxiao.EmployeeId == EmpinfoID)
                    {
                        rightView.MarketForecast = nowplan.MarketForecast;
                    }
                    //展示当前这个员工预计值
                    else
                    {
                        ChannelStaff alnoe = dbchannelstaff.GetChannelByEmpID(EmpinfoID);
                        EmployeesInfo empinfo = dbempstaff.GetInfoByEmpID(EmpinfoID);
                        List<ChannelArea> querylist = dbarea.GetAreasByPlan(nowplan, dbschoolpaln).Where(a => a.ChannelStaffID == alnoe.ID).ToList();
                        rightView.MarketForecast = querylist.Count * nowplan.AreaNumber;
                        rightView.personalempname = empinfo.EmpName;
                        rightView.personaldate = empinfo.EntryTime;
                        var resultregionname = "";
                        foreach (var item in querylist)
                        {
                            resultregionname+= dbregion.GetRegionByID(item.RegionID).RegionName+"、";
                        }
                        if (resultregionname.Length>1)
                        {
                            resultregionname = resultregionname.Substring(0, resultregionname.Length - 1);
                        }
                        rightView.personalregion = resultregionname;
                    }

                }
              
                else
                {
                    rightView.MarketForecast = nowplan.MarketForecast;
                }

            }

            //增长比  今年的-去年的=差距 差距/去年的*100
            if (string.IsNullOrEmpty(month))
            {
                //设置标题
                rightView.title = nowplan.Title;
                //设置top
                rightView.top = this.resulttopviewdata(Querylist, planid, month);
                //设置预计值

                foreach (var item in Querylist)
                {

                    var querybeiancount = dbbeian.GetBeanCount(item.EmployeesInfomation_Id, planid).Count;
                    var querygoschoolcount = dbbeian.GetGoSchoolByPlan(planid, item.EmployeesInfomation_Id).Count;
                    var querybaomingcount = dbbeian.GetBaoMingCount(item.EmployeesInfomation_Id, planid).Count;

                    nowbeiancount += querybeiancount;
                    nowgoshcoolcount += querygoschoolcount;
                    nowbaomingcount += querybaomingcount;

                }
                //年跟年之间的一个比较
                if (ThePreviousPlan.ID != 0)
                {
                    circleView.isExistData = true;
                   

                    //这是上一年的员工
                    var ThePreviousPlanData = this.Resultbytiaojian(EmpinfoID, IsTeam, ThePreviousPlan);

                    foreach (var item in ThePreviousPlanData)
                    {
                        thepreviousbeiancount += dbbeian.GetBeanCount(item.EmployeesInfomation_Id, ThePreviousPlan.ID).Count;
                        thepreviousgoshcoolcount += dbbeian.GetGoSchoolByPlan(ThePreviousPlan.ID, item.EmployeesInfomation_Id).Count;
                        thepreviousbaomingcount += dbbeian.GetBaoMingCount(item.EmployeesInfomation_Id, ThePreviousPlan.ID).Count;
                    }

                }
            }
            else
            {
                //月跟月
                circleView.isMonth = true;
                rightView.title = nowplan.Title + month + "月";

                //设置top
                rightView.top = this.resulttopviewdata(Querylist, planid, month);

                //所点击的月份
                int nowmonth = int.Parse(month);

                //当前点击月份员工产生的数据
                foreach (var item in Querylist)
                {
                    var list = dbbeian.GetBeanCount(item.EmployeesInfomation_Id, planid);
                    nowbeiancount += dbchannelstaff.GetChannelMonthKeepOnRecord(nowmonth, list).Count;
                    nowgoshcoolcount += dbchannelstaff.GetChannelGoSchoolCount(nowmonth, list).Count;
                    nowbaomingcount += dbchannelstaff.GetChannelSignUpCount(nowmonth, list).Count;
                }
                if (nowmonth== 1)
                {
                   
                    if (ThePreviousPlan != null)
                    {
                        circleView.isExistData = true;
                        //查询上一个季度的12月份
                        var ThePreviousPlanData = this.Resultbytiaojian(EmpinfoID, IsTeam, ThePreviousPlan);

                        foreach (var item in ThePreviousPlanData)
                        {
                            var list = dbbeian.GetBeanCount(item.EmployeesInfomation_Id, ThePreviousPlan.ID);
                            thepreviousbeiancount += dbchannelstaff.GetChannelMonthKeepOnRecord(12, list).Count;
                            thepreviousgoshcoolcount += dbchannelstaff.GetChannelGoSchoolCount(12, list).Count;
                            thepreviousbaomingcount += dbchannelstaff.GetChannelSignUpCount(12, list).Count;
                        }
                    }
                }

            }


            if (circleView.isExistData)
            {
                //算出来的比例  int/int是不会出现小数点，我们采用float

                float beianfenmu = float.Parse((nowbeiancount - thepreviousbeiancount).ToString());
                float goschoolfenmu = float.Parse((nowgoshcoolcount - thepreviousgoshcoolcount).ToString());
                float baomingfenmu = float.Parse((nowbaomingcount - thepreviousbaomingcount).ToString());

                circleView.beianRatio = Percentage(beianfenmu, float.Parse(thepreviousbeiancount.ToString()));
                circleView.goSchoolRatio = Percentage(goschoolfenmu, float.Parse(thepreviousgoshcoolcount.ToString()));
                circleView.baomingRatio = Percentage(baomingfenmu, float.Parse(thepreviousbaomingcount.ToString()));

                circleView.beianCount = beianfenmu;
                circleView.goSchoolCount = goschoolfenmu;
                circleView.baomingCount = baomingfenmu;
            }

            //设置备案量
            rightView.BeianCount = nowbeiancount;
            //设置上门量
            rightView.GoSchoolCount = nowgoshcoolcount;
            //设置报名量
            rightView.BaomingCount = nowbaomingcount;

            resultview.circleView = circleView;
            resultview.rightView = rightView;

            
                return Json(resultview, JsonRequestBehavior.AllowGet);
        }

        

        /// <summary>
        /// 用于找top榜的
        /// </summary>
        /// <param name="sortViews"></param>
        /// <param name="amounttype"></param>
        /// <returns></returns>
        public AmountTopView GetAmount(List<ChannelSortView> sortViews,string amounttype) {
            dbempstaff = new EmployeesInfoManage();
            int min;

            for (int i = 0; i < sortViews.Count - 1; i++)
            {
                min = i;
                for (int j = i + 1; j < sortViews.Count; j++)
                {
                    
                    switch (amounttype)
                    {
                        case "备案量":
                            if (sortViews[j].beiancount > sortViews[min].beiancount)
                                min = j;
                            break;

                        case "上门量":
                            if (sortViews[j].goschoolcount > sortViews[min].goschoolcount)
                                min = j;
                            break;

                        case "报名量":
                            if (sortViews[j].baomingcount > sortViews[min].baomingcount)
                                min = j;
                            break;
                    }
                }
                ChannelSortView t = sortViews[min];
                sortViews[min] = sortViews[i];
                sortViews[i] = t;
            }

            var toplist = sortViews.Take(3).ToList();

            AmountTopView topView = new AmountTopView();
            topView.countname = amounttype;


            if (toplist.Count > 0)
            {
                switch (amounttype)
                {
                    case "备案量":
                        topView.top1count = toplist[0].beiancount;
                        break;
                    case "上门量":
                        topView.top1count = toplist[0].goschoolcount;
                        break;
                    case "报名量":
                        topView.top1count = toplist[0].baomingcount;
                        break;

                }
                topView.top1name = dbempstaff.GetInfoByEmpID(toplist[0].channelStaff.EmployeesInfomation_Id).EmpName;
                topView.top2count = 0;
                topView.top2name = "无";
                topView.top3count = 0;
                topView.top3name = "无";
                if (toplist.Count > 1)
                {
                    switch (amounttype)
                    {
                        case "备案量":
                            topView.top2count = toplist[1].beiancount;
                            break;
                        case "上门量":
                            topView.top2count = toplist[1].goschoolcount;
                            break;
                        case "报名量":
                            topView.top2count = toplist[1].baomingcount;
                            break;

                    }
                    topView.top2name = dbempstaff.GetInfoByEmpID(toplist[1].channelStaff.EmployeesInfomation_Id).EmpName;
                    topView.top3count = 0;
                    topView.top3name = "无";
                    if (toplist.Count > 2)
                    {
                        switch (amounttype)
                        {
                            case "备案量":
                                topView.top3count = toplist[2].beiancount;
                                break;
                            case "上门量":
                                topView.top3count = toplist[2].goschoolcount;
                                break;
                            case "报名量":
                                topView.top3count = toplist[2].baomingcount;
                                break;

                        }
                        topView.top3name = dbempstaff.GetInfoByEmpID(toplist[2].channelStaff.EmployeesInfomation_Id).EmpName;
                    }
                }
            }
            else
            {
                topView.top1count = 0;
                topView.top1name = "无";
                topView.top2count = 0;
                topView.top2name = "无";
                topView.top3count = 0;
                topView.top3name = "无";
            }
                    
            return topView;
        }

        /// <summary>
        /// 返回三个量的top集合 整个计划
        /// </summary>
        /// <returns></returns>
        public List<AmountTopView> resulttopviewdata(List<ChannelStaff> Querylist, int planid, string month) {
            List<ChannelSortView> sortViews = new List<ChannelSortView>();
            foreach (var item in Querylist)
            {
                ChannelSortView ChannelSortView = new ChannelSortView();
                int querybeiancount;
                int querygoschoolcount;
                int querybaomingcount;
                if (string.IsNullOrEmpty(month))
                {
                    querybeiancount= dbbeian.GetBeanCount(item.EmployeesInfomation_Id, planid).Count;
                    querygoschoolcount= dbbeian.GetGoSchoolByPlan(planid, item.EmployeesInfomation_Id).Count;
                    querybaomingcount= dbbeian.GetBaoMingCount(item.EmployeesInfomation_Id, planid).Count;
                }
                else
                {
                    int nowmonth = int.Parse(month);
                    var list = dbbeian.GetBeanCount(item.EmployeesInfomation_Id, planid);
                    querybeiancount = dbchannelstaff.GetChannelMonthKeepOnRecord(nowmonth, list).Count;
                    querygoschoolcount = dbchannelstaff.GetChannelGoSchoolCount(nowmonth, list).Count;
                    querybaomingcount = dbchannelstaff.GetChannelSignUpCount(nowmonth, list).Count;
                }
                

                ChannelSortView.channelStaff = item;
                ChannelSortView.beiancount = querybeiancount;
                ChannelSortView.goschoolcount = querygoschoolcount;
                ChannelSortView.baomingcount = querybaomingcount;
                sortViews.Add(ChannelSortView);
            }
            List<AmountTopView> resulttop = new List<AmountTopView>();
            resulttop.Add(this.GetAmount(sortViews, "备案量"));
            resulttop.Add(this.GetAmount(sortViews, "上门量"));
            resulttop.Add(this.GetAmount(sortViews, "报名量"));
            return resulttop;
        }
        /// <summary>
        /// 判断是全体、团队还是个人
        /// </summary>
        /// <param name="EmpinfoID"></param>
        /// <param name="IsTeam"></param>
        /// <param name="nowplan"></param>
        /// <returns></returns>
        public List<ChannelStaff> Resultbytiaojian(string EmpinfoID, bool IsTeam, SchoolYearPlan nowplan)
        {
            dbchannelstaff = new ChannelStaffBusiness();

               //这是该年工作的员工
               var data = dbchannelstaff.GetChannelByYear(nowplan, dbschoolpaln);
            //返回的要当前查询的数据的员工
            List<ChannelStaff> Querylist = new List<ChannelStaff>();
            //如果是一个主任团队 那就肯定主任的员工编号
            if (IsTeam)
            {
                //判断有没有员工编号  有值就是选择的是主任
                if (!string.IsNullOrEmpty(EmpinfoID))
                {
                    Querylist = dbarea.GetTeamByEmpID(EmpinfoID, nowplan, data);

                }
                //显示全部
                else
                {
                    Querylist = data;
                }

            }
            //点击的是杨校就是显示全年的总体的效果或者是点击的是个人体现的是个人业绩
            else
            {
                //判断有没有员工编号
                if (!string.IsNullOrEmpty(EmpinfoID))
                {
                    //这个员工编号需要判断是不是杨校的员工编号
                    var yangxiao = dbempstaff.GetYangxiao();
                    //展示全年的整体的数据
                    if (yangxiao.EmployeeId == EmpinfoID)
                    {
                        Querylist = data;
                    }
                    //展示当前这个员工的这一年的业绩
                    else
                    {
                        var channelstaff = dbchannelstaff.GetChannelByEmpID(EmpinfoID);
                        Querylist.Add(channelstaff);
                    }

                }
                //显示全部
                else
                {
                    Querylist = data;
                }

            }

            return Querylist;
        }

        public float Percentage(float fenmu, float thepreviouscount)
        {
            float Ratio = 0;
            //分子为0
            if (thepreviouscount == 0)
            {
                //Infinity
                Ratio = fenmu * 100;
            }
            //分子分母为0
            else if (fenmu == 0 && thepreviouscount == 0)
            {
                //NaN
                Ratio = 0;
            }
            //分母为0
            else if (fenmu == 0)
            {
                //0
                Ratio = thepreviouscount * 100;
            }
            else if (fenmu < 0)
            {
                Ratio = fenmu * 100;
            }
            //正常情况
            else if (fenmu > 0)
            {
                Ratio = (fenmu / thepreviouscount) * 100;
            }

            return Ratio;
        }


        public ActionResult GetBaomingData(string myempid, bool myistema, int myplanid, string mymonth,int page,int limit)
        {
            dbschoolpaln = new SchoolYearPlanBusiness();
            dbempstaff = new EmployeesInfoManage();
            dbbeian = new StudentDataKeepAndRecordBusiness();
            dbteacher = new TeacherClassBusiness();
            dbheadermaster = new HeadmasterBusiness();
            dbclass = new ScheduleForTraineesBusiness();
            dbstudent = new StudentInformationBusiness();
            dbteacherClass = new TeacherClassBusiness();
            SchoolYearPlan nowplan = dbschoolpaln.GetPlanByID(myplanid);
            //根据选择的的条件获取对应的员工
            List<ChannelStaff> Querylist = this.Resultbytiaojian(myempid, myistema, nowplan);

            List<StudentPutOnRecord> baominglist = new List<StudentPutOnRecord>();

            List<ShowBaomingListView> resultdataviewlist = new List<ShowBaomingListView>();
            //月份为空 所以就是年整年的报名人数
            if (string.IsNullOrEmpty(mymonth))
            {
                foreach (var item in Querylist)
                {
                    var resultlist= dbbeian.GetBaoMingCount(item.EmployeesInfomation_Id, myplanid);
                    baominglist.AddRange(resultlist);
                }
            }
            else
            {
                int month = int.Parse(mymonth);
                foreach (var item in Querylist)
                {
                   
                    var list = dbbeian.GetBeanCount(item.EmployeesInfomation_Id, myplanid);
                    var resultlist= dbchannelstaff.GetChannelSignUpCount(month, list);
                    baominglist.AddRange(resultlist);
                }
            }
            var sutdentlist = dbstudent.GetIQueryable().ToList();
            foreach (var item in baominglist)
            {
                var empinfo= dbempstaff.GetInfoByEmpID(item.EmployeesInfo_Id);
                var student = sutdentlist.Where(a => a.IsDelete == false && a.StudentPutOnRecord_Id == item.Id).FirstOrDefault();
                var mrdteacher = dbteacherClass.GetTeacherByStudent1(student.StudentNumber);

                var headermaster = dbheadermaster.Listheadmasters(student.StudentNumber);

                ShowBaomingListView listView = new ShowBaomingListView();
                listView.BaomingDate = Convert.ToDateTime(item.StatusTime);
                listView.BeianDate = item.StuDateTime;
                listView.ChannelStaffName = empinfo.EmpName;
                listView.GoSchoolDate = Convert.ToDateTime(item.StuVisit);
                listView.StudentName = item.StuName;
                listView.OldSchoolName = item.StuSchoolName;
                listView.ProfessionalTeacher = mrdteacher.EmpName;

                if (headermaster==null)
                {
                    listView.Headmaster = "暂无班主任";
                }
                else
                {
                    listView.Headmaster = headermaster.EmpName;
                }
                
                listView.ClassNo = dbclass.SutdentCLassName(student.StudentNumber).ClassID;

                resultdataviewlist.Add(listView);


            }


            var bnewdata = resultdataviewlist.Skip((page - 1) * limit).Take(limit).ToList();
            var returnObj = new
            {
                code = 0,
                msg = "",
                count = resultdataviewlist.Count(),
                data = bnewdata
            };

            return Json(returnObj, JsonRequestBehavior.AllowGet);

        }

 
    }
}