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
            var rdslist = rptmanage.GetList();
            var myrdslist = rdslist.OrderBy(r => r.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var newlist = from rds in myrdslist
                          select new
                          {
                              #region 赋值
                              rds.Id,
                              rds.Name,
                              pname = GetPosition((int)rds.Pid).PositionName,
                              rds.PhoneNumber,
                              rds.TraceTime,
                              rds.Channel,
                              rds.ResumeType,
                              rds.PhoneCommunicateResult,
                              rds.FirstInterviewDate,
                              rds.IsInterview,
                              rds.FirstInterviewResult,
                              rds.RetestDate,
                              rds.RetestResult,
                              rds.OfferGiveTime,
                              rds.PlanEntryTime,
                              rds.IsEntry,
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

        //获取月度招聘数据汇总
        public ActionResult GetRecruitData(int page, int limit,string AppCondition) {
             AddRecruitData();
            RecruitingDataSummaryManage rdsmanage = new RecruitingDataSummaryManage();
            var rdslist = rdsmanage.GetList();
            var myrdslist = rdslist.OrderBy(r => r.Id).Skip((page - 1) * limit).Take(limit).ToList();
            if (!string.IsNullOrEmpty(AppCondition))
            {
                string[] str = AppCondition.Split(',');
                string pname = str[0];
                string start_time = str[1];
                string end_time = str[2];
                if (!string.IsNullOrEmpty(pname) )
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
                    DateTime etime = Convert.ToDateTime(end_time );
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
        /// <summary>
        /// 根据员名称获取员工编号
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string GetEmpidByName(string name) {
            EmployeesInfoManage emanage = new EmployeesInfoManage();
            var empid = emanage.GetList().Where(e => e.EmpName == name).FirstOrDefault().EmployeeId;
            return empid;
        }


        //添加招聘电话追踪信息
        public ActionResult Addrpt() {
            return View();
        }
        //添加招聘电话追踪记录页面绑定岗位属性下拉框
        public ActionResult BindPidSelect()
        {
            PositionManage pmanage = new PositionManage();
            var emp = pmanage.GetList();
            var newobj = new
            {
                code = 0,
                msg = "",
                count = emp.Count(),
                data = emp
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Addrpt(RecruitPhoneTrace rpt) {
            RecruitPhoneTraceManage rmanage = new RecruitPhoneTraceManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                
                rpt.IsDel = false;
                rmanage.Insert(rpt);
                AjaxResultxx = rmanage.Success();

            }
            catch (Exception ex)
            {
                AjaxResultxx = rmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //招聘电话追踪记录的是否入职属性修改
        public ActionResult EditRptIsentry(int id, bool isdel) {
            RecruitPhoneTraceManage rmanage = new RecruitPhoneTraceManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var rpt = rmanage.GetEntity(id);
                rpt.IsEntry = isdel;
                rmanage.Update(rpt);
                AjaxResultxx = rmanage.Success();

            }
            catch (Exception ex)
            {
                AjaxResultxx = rmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //招聘电话追踪记录的是否面试属性修改
        public ActionResult EditRptIsInterview(int id, bool isdel) {
            RecruitPhoneTraceManage rmanage = new RecruitPhoneTraceManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var rpt = rmanage.GetEntity(id);
                rpt.IsInterview = isdel;
                rmanage.Update(rpt);
                AjaxResultxx = rmanage.Success();

            }
            catch (Exception ex)
            {
                AjaxResultxx = rmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

        public  string Condition(DateTime date, string type)
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
        /// 招聘电话追踪编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Editrpt(int id) {
            RecruitPhoneTraceManage rpt = new RecruitPhoneTraceManage();
             var r= rpt.GetEntity(id);
            PositionManage pmanage = new PositionManage();
            ViewBag.pname = new SelectList(pmanage.GetList(),"Pid","PositionName");
            ViewBag.id = id;
            return View(r);
        }
        public ActionResult GetrptById(int Id) {
            RecruitPhoneTraceManage rpt = new RecruitPhoneTraceManage();
            var r = rpt.GetEntity(Id);
            return Json(r,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Editrpt(RecruitPhoneTrace rpt) {
            RecruitPhoneTraceManage rmanage = new RecruitPhoneTraceManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                rmanage.Update(rpt);
                AjaxResultxx= rmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = rmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
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
                               resumenum = g.Count(),//简历总数
                               PhoneCommunicatenum= g.Count(t=>t.PhoneCommunicateResult != null),//电话呼出总数
                               invitednum=g.Count(t=>t.FirstInterviewDate!=null),//邀约总数
                               Facednum=g.Count(t=>t.IsInterview==true),//当月到面总数
                               Refacednum=g.Count(t=>t.RetestResult!="-1"),//当月复试总数
                               Refacepassednum=g.Count(t=>t.RetestResult=="通过"),//当月复试通过总数
                               Entrynum=g.Count(t=>t.IsEntry==true)//当月入职人数
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
                        item.InstantToFacesRate = (decimal)s.Facednum / (decimal)s.invitednum;
                    }
                    if (s.PhoneCommunicatenum != 0)
                    {
                        item.InstantInviteRate = (decimal)s.invitednum / (decimal)s.PhoneCommunicatenum;
                    }
                    if (s.Refacednum != 0)
                    {
                        item.InstantRetestPassrate = (decimal)s.Refacepassednum / (decimal)s.Refacednum;
                    }
                    if (s.Refacepassednum != 0)
                    {
                        item.EntryRate = (decimal)s.Entrynum / (decimal)s.Refacepassednum;
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
                        rds.RecruitPercentage = (decimal)rds.InstantEntryNum / (decimal)rds.PlanRecruitNum;
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