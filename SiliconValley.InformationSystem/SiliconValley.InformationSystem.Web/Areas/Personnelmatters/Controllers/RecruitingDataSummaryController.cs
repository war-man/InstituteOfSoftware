using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    using SiliconValley.InformationSystem.Business.RecruitingDataSummaryBusiness;
    using SiliconValley.InformationSystem.Business.TalentDemandPlanBusiness;
    using SiliconValley.InformationSystem.Business.PositionBusiness;
    using SiliconValley.InformationSystem.Business.DepartmentBusiness;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Business.RecruitPhoneTraceBusiness;

    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Util;
    using System.Text;
    using System.IO;
    using SiliconValley.InformationSystem.Business.Common;
    using SiliconValley.InformationSystem.Entity.Base_SysManage;
    using System.Data;
    using SiliconValley.InformationSystem.Entity.ViewEntity;

    public class RecruitingDataSummaryController : Controller
    {
        // GET: Personnelmatters/RecruitingDataSummary
        public ActionResult RecruitIndex()
        {
            return View();
        }
       
        //获取招聘电话追踪数据
        public ActionResult GetTraceData(int page, int limit)
        {  
            RecruitPhoneTraceManage rptmanage = new RecruitPhoneTraceManage();
            List<RecruitPhoneTraceView> rptviewlist = new List<RecruitPhoneTraceView>();
          
            var rdslist = rptmanage.GetList().Where(s=>s.IsDel==false).ToList();         
            foreach (var item in rdslist)
            {
                RecruitPhoneTraceView rptview = rptmanage.GetRptView(item.Id);
                rptviewlist.Add(rptview);
            }
            var myrdslist = rptviewlist.OrderBy(r => r.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var newobj = new
            {
                code = 0,
                msg = "",
                count = rptviewlist.Count(),
                data = myrdslist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

      
        #region 获取某个部门或岗位
       
        /// <summary>
        /// 获取部门对象
        /// </summary>
        /// <param name="deptid"></param>
        /// <returns></returns>
        public Department GetDept(int deptid) {
            DepartmentManage dmanage = new DepartmentManage();
            var deptobj = dmanage.GetEntity(deptid);
            return deptobj;
        }

        /// <summary>
        ///获取所属岗位对象
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public Position GetPosition(int pid)
        {
            PositionManage pmanage = new PositionManage();
            var str = pmanage.GetEntity(pid);
            return str;
        }

        /// <summary>
        /// 根据部门名称获取部门编号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetDeptidByName(string name) {
            DepartmentManage dmanage = new DepartmentManage();
            var dept = dmanage.GetList().Where(s => s.DeptName == name).FirstOrDefault();
            return dept.DeptId;
        }
        /// <summary>
        /// 根据岗位名称获取岗位编号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetPidByName(string name) {
            PositionManage pmanage = new PositionManage();
            var pid = pmanage.GetList().Where(p => p.PositionName == name).FirstOrDefault().Pid;
            return pid;
        }
        #endregion


        /// <summary>
        /// 通过编号获取某条招聘记录数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult GetRPTById(int id) {
            RecruitPhoneTraceManage rptmanage = new RecruitPhoneTraceManage();
            var rptview = rptmanage.GetRptView(id);
            return Json(rptview,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加招聘电话追踪基本信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Addrpt() {
            return View();
        }
        [HttpPost]
        public ActionResult Addrpt(RecruitPhoneTrace rpt) {
            RecruitPhoneTraceManage rmanage = new RecruitPhoneTraceManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                rpt.IsEntry = false;
                rpt.IsDel = false;
                rpt.PhoneCommunicateResult = false;
                rmanage.Insert(rpt);
                AjaxResultxx = rmanage.Success();
              
            }
            catch (Exception ex)

            {
                AjaxResultxx = rmanage.Error(ex.Message);
            }
            if (AjaxResultxx.Success)
            {
                rpt.SonId = rpt.Id;
                rmanage.Update(rpt);
                AjaxResultxx = rmanage.Success();
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加招聘的面试记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddTrack(int id) {
            RecruitPhoneTraceManage rmanage = new RecruitPhoneTraceManage();
            ViewBag.Id = id;
            var rds = rmanage.GetEntity(id);
            var rdslist = rmanage.GetList().Where(r => r.SonId == rds.SonId).ToList();
            ViewBag.Number = rdslist.Count()-1;
           // ViewBag.rdslist = rdslist;
            return View();
        }
        [HttpPost]
        public ActionResult AddTrack(RecruitPhoneTrace rpt) {
            RecruitPhoneTraceManage rptmanage = new RecruitPhoneTraceManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var beforerpt = rptmanage.GetList().Where(r => r.Id==rpt.Id && r.IsDel == false).FirstOrDefault();
                RecruitPhoneTrace rptnew = new RecruitPhoneTrace();
                rptnew.SonId = beforerpt.SonId;
                rptnew.Name = beforerpt.Name;
                rptnew.PhoneNumber = beforerpt.PhoneNumber;
                rptnew.TraceTime = rpt.TraceTime;
                rptnew.ForwardDate = rpt.ForwardDate;
                rptnew.PhoneCommunicateResult =rpt.PhoneCommunicateResult;
                rptnew.Channel = beforerpt.Channel;
                rptnew.ResumeType = beforerpt.ResumeType;
                rptnew.IsEntry = beforerpt.IsEntry;
                rptnew.Remark = rpt.Remark;
                rptnew.IsDel = true;
                rptnew.Pid = rpt.Pid;
             
               rptmanage.Insert(rptnew);
                AjaxResultxx = rptmanage.Success();
                if (AjaxResultxx.Success) {
                    beforerpt.PhoneCommunicateResult = rptnew.PhoneCommunicateResult;
                    rptmanage.Update(beforerpt);
                    AjaxResultxx = rptmanage.Success();
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = rptmanage.Error(ex.Message);
            }
           
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑某条招聘追踪数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditTrack(int id) {
            RecruitPhoneTraceManage rmanage = new RecruitPhoneTraceManage();
            List<RecruitPhoneTraceView> rptviewlist = new List<RecruitPhoneTraceView>();

            ViewBag.Id = id;
            rptviewlist = rmanage.GetRptViewList(id);//获取回访记录集合
            ViewBag.rptviewlist = rptviewlist;
            ViewBag.Number = rptviewlist.Count();
            var rpt = rmanage.GetRptView(id);
            ViewBag.pid = rpt.Pid;
            ViewBag.pname = rpt.Pname;
            return View(rpt);
        }
        /// <summary>
        /// 没有面试记录的追踪编辑提交
        /// </summary>
        /// <param name="Tracklist"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTracks(string Tracklist) {
            var AjaxResultxx = new AjaxResult();
            RecruitPhoneTraceManage rptmanage = new RecruitPhoneTraceManage();
            try
            {
                string[] str = Tracklist.Split(',');
                    string id = str[0];
                    string pid = str[1];
                    string time = str[2];
                    string PhoneNumber = str[3];
                    string Channel = str[4];
                    string ResumeType = str[5];
                    string remark = str[6];
                var rpt = rptmanage.GetEntity(int.Parse(id));
                rpt.Pid =int.Parse(pid);
                rpt.TraceTime =Convert.ToDateTime(time);
                rpt.PhoneNumber = PhoneNumber;
                rpt.Channel = Channel;
                rpt.ResumeType = ResumeType;
                rpt.Remark = remark;
                rptmanage.Update(rpt);
                AjaxResultxx = rptmanage.Success();

            }
            catch (Exception ex)
            {
                AjaxResultxx = rptmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 有面试记录的追踪编辑提交
        /// </summary>
        /// <param name="mydata"></param>
        /// <param name="Tracklist"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditTrack(RecruitPhoneTrace mydata, string Tracklist) {
            var AjaxResultxx = new AjaxResult();
            RecruitPhoneTraceManage rptmanage = new RecruitPhoneTraceManage();
            try
            {     
                var rpt1 = rptmanage.GetEntity(mydata.Id);
                rpt1.TraceTime = mydata.TraceTime;
                rpt1.PhoneCommunicateResult = mydata.PhoneCommunicateResult;
                rpt1.Remark = mydata.Remark;
                rptmanage.Update(rpt1);
                AjaxResultxx = rptmanage.Success();
                if (AjaxResultxx.Success) {
                    string[] str = Tracklist.Split(',');
                    string id = str[0];
                    string pid = str[1];
                    string time = str[2];
                    string PhoneNumber = str[3];
                    string Channel = str[4];
                    string ResumeType = str[5];
                   // string result = str[6];
                    string remark = str[6];
                    var rpt2 = rptmanage.GetEntity(int.Parse(id));
                    rpt2.Pid = int.Parse(pid);
                    rpt2.TraceTime = Convert.ToDateTime(time);
                    rpt2.PhoneNumber = PhoneNumber;
                    rpt2.Channel = Channel;
                    rpt2.ResumeType = ResumeType;
                  //  rpt2.PhoneCommunicateResult =Convert.ToBoolean(result);
                    rpt2.Remark = remark;
                    rpt2.PhoneCommunicateResult = rpt1.PhoneCommunicateResult;
                    rptmanage.Update(rpt2);
                    AjaxResultxx = rptmanage.Success();
                 
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = rptmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 追踪记录详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult TrackDetailInfo(int id) {
            RecruitPhoneTraceManage rmanage = new RecruitPhoneTraceManage();
            List<RecruitPhoneTraceView> rptviewlist = new List<RecruitPhoneTraceView>();
            ViewBag.Id = id;
            rptviewlist = rmanage.GetRptViewList(id);//获取回访记录集合
            ViewBag.rptviewlist = rptviewlist;
            ViewBag.Number = rptviewlist.Count();
            return View();
        }

        public ActionResult IsEntry(int id) {
            RecruitPhoneTraceManage rmanage = new RecruitPhoneTraceManage();
            var rpt = rmanage.GetRptView(id);
            ViewBag.Id = id;
            ViewBag.pid = rpt.Pid;
            ViewBag.pname = rpt.Pname;
            return View();
        }
        [HttpPost]
        public ActionResult IsEntry(string id) {
            RecruitPhoneTraceManage rmanage = new RecruitPhoneTraceManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var rpt = rmanage.GetEntity(int.Parse(id));
                var rptlist = rmanage.GetList().Where(s => s.SonId == rpt.SonId).ToList();
                foreach (var item in rptlist)
                {
                    item.IsEntry = true;
                    rmanage.Update(item);
                }
               AjaxResultxx= rmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = rmanage.Error(ex.Message);
            }
           
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }


        //获取月度招聘数据汇总
        public ActionResult GetRecruitData(int page, int limit, string AppCondition)
        {
            AddRecruitData();
            RecruitingDataSummaryManage rdsmanage = new RecruitingDataSummaryManage();
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var rdslist = rdsmanage.GetList();
            var myrdslist = rdslist.OrderBy(r => r.Id).Skip((page - 1) * limit).Take(limit).ToList();
            if (!string.IsNullOrEmpty(AppCondition))
            {
                string[] str = AppCondition.Split(',');
                string deptid = str[0];
                string pname = str[1];
                string start_time = str[2];
                string end_time = str[3];
                if (!string.IsNullOrEmpty(deptid))
                {
                    myrdslist = myrdslist.Where(e => empmanage.GetDeptByPid((int)e.Pid).DeptId == int.Parse(deptid)).ToList();
                }
                if (!string.IsNullOrEmpty(pname))
                {
                    myrdslist = myrdslist.Where(e => e.Pid == int.Parse(pname)).ToList();
                }
                if (!string.IsNullOrEmpty(start_time))
                {
                    DateTime stime = Convert.ToDateTime(start_time);
                    myrdslist = myrdslist.Where(a => a.YearAndMonth >= stime).ToList();
                }
                if (!string.IsNullOrEmpty(end_time))
                {
                    DateTime etime = Convert.ToDateTime(end_time);
                    myrdslist = myrdslist.Where(a => a.YearAndMonth <= etime).ToList();
                }
            }
            var newlist = from rds in myrdslist
                          select new
                          {
                              #region 赋值
                              rds.Id,
                              rds.YearAndMonth,
                              pname = GetPosition((int)rds.Pid).PositionName,
                              rds.PlanRecruitNum,
                              rds.ResumeSum,
                              rds.OutboundCallSum,
                              rds.InstantInviteSum,
                              rds.InstantToFacesSum,
                              rds.InstantRetestSum,
                              rds.InstantRetestPassSum,
                              rds.InstantEntryNum,
                              rds.InstantToFacesRate,
                              rds.InstantInviteRate,
                              rds.InstantRetestPassrate,
                              rds.EntryRate,
                              rds.RecruitPercentage,
                              rds.Remark
                              #endregion
                          };
            var newobj = new
            {
                code = 0,
                msg = "",
                count = rdslist.Count(),
                data = newlist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }


        public string Condition(DateTime date, string type)
        {
            if (type == "day")
            {
                return date.ToString("yyyy-M-d");
            }
            else if (type == "month")
            {
                return date.ToString("yyyy-M");
            }
            return date.Year.ToString();
        }

        /// <summary>
        ///为计算某月某岗位的复试总数
        /// </summary>
        /// <param name="rptlist"></param>
        /// <returns></returns>
        public int GetRefacednum(List<RecruitPhoneTrace> rptlist) {
            int result=0;
            var conditionrpt = rptlist.GroupBy(s=>s.SonId);

            conditionrpt.ForEach(d=>
            {
                if (d.Count() > 1)
                {
                    result+=d.Count()-1;
                }
                
            });

           // result = conditionrpt.Count();
            return result;
        }
        //月度招聘数据汇总添加
        public AjaxResult AddRecruitData()   {
            var AjaxResultxx = new AjaxResult();
            RecruitPhoneTraceManage rpt = new RecruitPhoneTraceManage();
            RecruitingDataSummaryManage rdsmanage = new RecruitingDataSummaryManage();
            try
            {
                var list = from r in rpt.GetList()
                           group r by new
                           { r.Pid, month = Condition((DateTime)r.TraceTime, "month") } into g
                           select new
                           {
                               month = g.Key.month,//月份
                               position = g.Key.Pid,//岗位
                               resumenum = g.Count(s => s.IsDel == false),//简历总数(排除这些应聘者的面试记录)
                               PhoneCommunicatenum=g.Count(s => s.IsDel == false && !string.IsNullOrEmpty(Convert.ToString(s.TraceTime))),//联系的总数(有联系时间的才算联系过)
                               invitednum=g.Count(t=>t.IsDel==false && t.ForwardDate != null),//当月邀约总数
                               Facednum=g.Count(t=>t.IsDel==true),//当月到面总数
                               Refacednum = GetRefacednum(g.Where(s=>s.IsDel==true).ToList()),//当月复试总数
                               Refacepassednum= GetRefacednum(g.Where(s => s.IsDel == true && s.PhoneCommunicateResult == true).ToList()),//当月复试通过总数
                               Entrynum=g.Count(t=>t.IsDel==false && t.IsEntry==true)//当月入职人数
                           };
                List<RecruitingDataSummary> rlist = new List<RecruitingDataSummary>();
                foreach (var s in list)
                {
                    #region 给对象赋值
                    RecruitingDataSummary item = new RecruitingDataSummary();
                    item.YearAndMonth = DateTime.Parse(s.month);
                    item.Pid = s.position;
                    item.ResumeSum = s.resumenum;
                    item.OutboundCallSum = s.PhoneCommunicatenum;
                    item.InstantInviteSum = s.invitednum;
                    item.InstantToFacesSum = s.Facednum;
                    item.InstantRetestSum = s.Refacednum;
                    item.InstantRetestPassSum = s.Refacepassednum;
                    item.InstantEntryNum = s.Entrynum;
                    if (s.invitednum != 0)
                    {
                        item.InstantToFacesRate = Convert.ToDecimal(s.Facednum) / Convert.ToDecimal(s.invitednum);
                    }
                    if (s.PhoneCommunicatenum != 0)
                    {
                        item.InstantInviteRate = Convert.ToDecimal(s.invitednum) / Convert.ToDecimal(s.PhoneCommunicatenum);
                    }
                    if (s.Refacednum != 0)
                    {
                        item.InstantRetestPassrate = Convert.ToDecimal(s.Refacepassednum) / Convert.ToDecimal(s.Refacednum);
                    }
                    if (s.Refacepassednum != 0)
                    {
                        item.EntryRate = Convert.ToDecimal(s.Entrynum) / Convert.ToDecimal(s.Refacepassednum);
                    }
                    var rds = rdsmanage.GetList().Where(a => a.Pid == item.Pid && Condition((DateTime)a.YearAndMonth, "month") == Condition((DateTime)item.YearAndMonth, "month")).FirstOrDefault();
                    if (rds != null)
                    {
                        rds.YearAndMonth = item.YearAndMonth;
                        rds.Pid = item.Pid;
                        rds.ResumeSum = item.ResumeSum;
                        rds.OutboundCallSum = item.OutboundCallSum;
                        rds.InstantInviteSum = item.InstantInviteSum;
                        rds.InstantToFacesSum = item.InstantToFacesSum;
                        rds.InstantRetestSum = item.InstantRetestSum;
                        rds.InstantRetestPassSum = item.InstantRetestPassSum;
                        rds.InstantEntryNum = item.InstantEntryNum;
                        rds.InstantToFacesRate = item.InstantToFacesRate;
                        rds.InstantInviteRate = item.InstantInviteRate;
                        rds.InstantRetestPassrate = item.InstantRetestPassrate;
                        rds.EntryRate = item.EntryRate;
                        rds.RecruitPercentage = Convert.ToDecimal(rds.InstantEntryNum) / Convert.ToDecimal(rds.PlanRecruitNum);
                        rdsmanage.Update(rds);
                    }
                    else if (rdsmanage.GetList().Count() == 0)
                    {
                        rdsmanage.Insert(item);
                    }
                    else
                    {
                        rdsmanage.Insert(item);
                    }
                    #endregion
                }
                AjaxResultxx = rdsmanage.Success();
                        
            }
            catch (Exception ex)
            {
                AjaxResultxx = rdsmanage.Error(ex.Message);
            }
            return AjaxResultxx;
        }
        /// <summary>
        /// 招聘数据汇总[计划招聘人数]字段的单元格编辑方法
        /// </summary>
        /// <param name="id"></param>
        /// <param name="endvalue"></param>
        /// <returns></returns>
        public ActionResult EditTableCell(int id,string attribute, string endvalue) {
           RecruitingDataSummaryManage rdsmanage = new RecruitingDataSummaryManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var rds= rdsmanage.GetEntity(id);
                switch (attribute)
                {
                    case "PlanRecruitNum":
                        rds.PlanRecruitNum = int.Parse(endvalue);
                        rds.RecruitPercentage = (decimal)rds.InstantEntryNum / (decimal)rds.PlanRecruitNum;
                        rdsmanage.Update(rds);
                        break;
                    case "Remark":
                        rds.Remark = endvalue;
                        rdsmanage.Update(rds);
                        break;
                }                          
                AjaxResultxx= rdsmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = rdsmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }


    }
}