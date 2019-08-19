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
                              dname=GetDept((int)tdp.DeptId).DeptName,
                              pname=GetPosition((int)tdp.Pid).PositionName,
                              tdp.DemandPersonNum,
                              tdp.PlanEntryTime,
                              tdp.PositionStatement,
                              tdp.PositionRequest,
                              tdp.RecruitReason,
                              empname=GetEmp(tdp.EmployeeId).EmpName,
                              tdp.ApplyTime,
                              tdp.Remark
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
        public Department GetDept(int deptid){
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

    }
}