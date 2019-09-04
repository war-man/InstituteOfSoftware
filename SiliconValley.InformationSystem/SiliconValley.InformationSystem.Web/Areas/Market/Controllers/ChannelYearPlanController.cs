using SiliconValley.InformationSystem.Business.Channel;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Business.Psychro;
using SiliconValley.InformationSystem.Business.StudentKeepOnRecordBusiness;
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
        // GET: Market/ChannelYearPlan
        public ActionResult ChannelYearPlanIndex()
        {
            dbschoolpaln = new SchoolYearPlanBusiness();
            var data = dbschoolpaln.GetAll().OrderByDescending(a => a.ID).Select(a => new ShowyearnameView
            {
                SchoolPlanID = a.ID,
                ShowTitle = a.Title
            }).ToList();
            ViewBag.YearName = data;
            ViewBag.NowYearName = data.FirstOrDefault();
            return View();
        }
        /// <summary>
        /// 表格数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ChannelYearPlanData(int page, int limit, int? PlanID, string EmpName)
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
            var planinfo = dbschoolpaln.GetPlanByID(selectplan);
            //获取该年有的人
            var data = dbchannelstaff.GetChannelByYear(selectplan, dbschoolpaln);
            List<MrdChannelYearPlanIndexView> mrdplan = new List<MrdChannelYearPlanIndexView>();
            foreach (var item in data)
            {
                //获取这个渠道员工这一年的上门量
                var goschoolcount = dbbeian.GetGoSchoolByPlan(selectplan, item.EmployeesInfomation_Id);
                //获取这个渠道员工这一年的备案量
                var beiancount = dbbeian.GetBeanCount(item.EmployeesInfomation_Id, selectplan);
                //获取这个渠道员工这一年的学员报名量
                var baomingcount = dbbeian.GetBaoMingCount(item.EmployeesInfomation_Id, selectplan);
                //获取借资量与预资量
                var debitcount = dbdebit.GetDebitsByYear(item.EmployeesInfomation_Id, planinfo);
                var fundingcoung = dbfunding.GetPrefundingsByYear(item.EmployeesInfomation_Id,planinfo);

                //获取员工该年度计划负责区域
                var channelarea= dbarea.GetAreaByPaln(item.ID, planinfo);

                Region resultregion = new Region();
                if (channelarea== null)
                {
                    resultregion.ID = 0;
                    resultregion.RegionName = "";
                }
                else
                {
                    //获取区域对象
                   resultregion = dbregion.GetRegionByID(channelarea.RegionID);
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
                mrdChannel.Region = resultregion.RegionName;
                mrdChannel.RegionID = resultregion.ID;
                mrdChannel.SignUpNumber = baomingcount.Count;
                mrdChannel.QuitDate = item.QuitDate;
                mrdChannel.EmpStaffID = item.EmployeesInfomation_Id;
                mrdChannel.DebitNumber = debitcount.Count+ fundingcoung.Count;
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
    }
}