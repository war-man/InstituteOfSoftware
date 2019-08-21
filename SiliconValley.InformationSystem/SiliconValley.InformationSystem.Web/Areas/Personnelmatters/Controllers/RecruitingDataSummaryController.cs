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

    public class RecruitingDataSummaryController : Controller
    {
        // GET: Personnelmatters/RecruitingDataSummary
        public ActionResult RecruitIndex()
        {
            return View();
        }
        //获取人才需求计划
        public ActionResult GetTalantDemandData(int page, int limit) {
            TalentDemandPlanManage tdpmanage = new TalentDemandPlanManage();
            var list = tdpmanage.GetList();
            var mytdplist = list.OrderBy(r => r.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var newlist = from tdp in mytdplist
                          select new {
                              tdp.Id,
                              dname = GetDept((int)tdp.DeptId).DeptName,
                              pname = GetPosition((int)tdp.Pid).PositionName,
                              tdp.DemandPersonNum,
                              tdp.PlanEntryTime,
                              tdp.PositionStatement,
                              tdp.PositionRequest,
                              tdp.RecruitReason,
                              empname = GetEmp(tdp.EmployeeId).EmpName,
                              tdp.ApplyTime,
                              tdp.Remark,
                              tdp.IsDel
                          };
            var newobj = new
            {
                code = 0,
                msg = "",
                count = list.Count(),
                data = newlist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
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
        public ActionResult GetRecruitData(int page, int limit) {
            RecruitingDataSummaryManage rdsmanage = new RecruitingDataSummaryManage();
            var rdslist = rdsmanage.GetList();
            var myrdslist = rdslist.OrderBy(r => r.Id).Skip((page - 1) * limit).Take(limit).ToList();
            var newobj = new
            {
                code = 0,
                msg = "",
                count = rdslist.Count(),
                data = rdslist
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }



        //获取部门对象
        public Department GetDept(int deptid) {
            DepartmentManage dmanage = new DepartmentManage();
            var deptobj = dmanage.GetEntity(deptid);
            return deptobj;
        }
        //获取所属岗位对象
        public Position GetPosition(int pid)
        {
            PositionManage pmanage = new PositionManage();
            var str = pmanage.GetEntity(pid);
            return str;
        }
        //获取员工（人才需求计划表-负责人）
        public EmployeesInfo GetEmp(string empid) {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var emp = empmanage.GetEntity(empid);
            return emp;
        }


        //添加人才需求计划
        public ActionResult AddTalantDemand() {
            return View();
        }
        //人才需求添加页绑定负责人属性下拉框
        public ActionResult BindEmpSelect() {
            EmployeesInfoManage empmanage = new EmployeesInfoManage();
            var emp = empmanage.GetList();
            var newobj = new {
                code = 0,
                msg = "",
                count = emp.Count(),
                data = emp
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult AddTalantDemand(TalentDemandPlan tdp) {
            TalentDemandPlanManage tdpmanage = new TalentDemandPlanManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                tdp.IsDel = false;
                tdpmanage.Insert(tdp);
                AjaxResultxx = tdpmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = tdpmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }
        //人才需求计划的是否完成招聘属性修改
        public ActionResult EditTdpIsdel(int id, bool isdel)
        {
            TalentDemandPlanManage tdpmanage = new TalentDemandPlanManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                var tdp = tdpmanage.GetEntity(id);
                tdp.IsDel = isdel;
                tdpmanage.Update(tdp);
                AjaxResultxx = tdpmanage.Success();
            }
            catch (Exception ex)
            {
                AjaxResultxx = tdpmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
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


        //月度招聘数据汇总添加
        public AjaxResult AddRecruitData() {
            var AjaxResultxx = new AjaxResult();

            return AjaxResultxx;
        }
    }
}