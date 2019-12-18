using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
using SiliconValley.InformationSystem.Entity.ViewEntity.SalaryView;
using SiliconValley.InformationSystem.Util;

namespace SiliconValley.InformationSystem.Web.Areas.Personnelmatters.Controllers
{
    public class EmpSalaryManagementController : Controller
    {
        //员工工资管理页面
        // GET: Personnelmatters/EmpSalaryManagement
        public ActionResult SalaryManageIndex()
        {
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资
            var time = msrmanage.GetList().Where(s=>s.IsDel==false).FirstOrDefault().YearAndMonth;
            string mytime = DateTime.Parse(time.ToString()).Year + "年" + DateTime.Parse(time.ToString()).Month + "月";
            ViewBag.yearandmonth = mytime;
            return View();
        }
        //工资表数据加载
        public ActionResult EmpSalaryData(int page, int limit,string AppCondition)
        {
            EmplSalaryEmbodyManage empsemanage = new EmplSalaryEmbodyManage();//员工工资体系表
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资
            EmployeesInfoManage empmanage = new EmployeesInfoManage();//员工信息表
            var eselist = msrmanage.GetList().Where(s => s.IsDel == false).ToList();
            if (!string.IsNullOrEmpty(AppCondition))
            {
                string[] str = AppCondition.Split(',');
                string ename = str[0];
                string deptname = str[1];
                string pname = str[2];

                eselist = eselist.Where(e => empmanage.GetInfoByEmpID(e.EmployeeId).EmpName.Contains(ename)).ToList();
                if (!string.IsNullOrEmpty(deptname))
                {
                    eselist = eselist.Where(e => empmanage.GetDeptByEmpid(e.EmployeeId).DeptId == int.Parse(deptname)).ToList();
                }
                if (!string.IsNullOrEmpty(pname))
                {
                    eselist = eselist.Where(e => empmanage.GetPositionByEmpid(e.EmployeeId).Pid == int.Parse(pname)).ToList();
                }
               
            }
            var newlist = eselist.OrderBy(s => s.Id).Skip((page - 1) * limit).Take(limit).ToList();
            List<MySalaryObjView> result = new List<MySalaryObjView>();
            
            foreach (var item in newlist)
            {
                MySalaryObjView view = new MySalaryObjView();
                view.Id = item.Id;
                view.EmployeeId = item.EmployeeId;
                view.empName = empmanage.GetEntity(item.EmployeeId).EmpName;
                view.Depart = empmanage.GetDeptByEmpid(item.EmployeeId).DeptName;
                view.Position = empmanage.GetPositionByEmpid(item.EmployeeId).PositionName;
              
                
               
                //员工工资体系表
                var eseobj= msrmanage.GetEmpsalaryByEmpid(item.EmployeeId);
                view.baseSalary = eseobj.BaseSalary;
                view.positionSalary = eseobj.PositionSalary;
                if (msrmanage.GetMCByEmpid(item.EmployeeId, (DateTime)item.YearAndMonth) == null)
                {
                    view.finalGrade =null;
                 
                }
                else {
                    view.finalGrade = msrmanage.GetMCByEmpid(item.EmployeeId, (DateTime)item.YearAndMonth).FinalGrade;
                   
                }
                if (view.finalGrade == null)
                {
                    view.PerformanceSalary = null;
                }
                else {
                    view.PerformanceSalary =msrmanage.GetempPerformanceSalary(view.finalGrade,eseobj.PerformancePay);
                }
              
                view.netbookSubsidy = eseobj.NetbookSubsidy;
                view.socialSecuritySubsidy = eseobj.SocialSecuritySubsidy;
                #region 应发工资1赋值
                var one = view.baseSalary + view.positionSalary;
              
                view.SalaryOne = msrmanage.GetSalaryone(one,view.PerformanceSalary,view.netbookSubsidy,view.socialSecuritySubsidy);
                #endregion
                //考勤表对象
                var attendobj = msrmanage.GetAttendanceInfoByEmpid(item.EmployeeId, (DateTime)item.YearAndMonth);
                if (attendobj == null)
                {
                    view.toRegularDays = null;
                    view.leavedays = null;
                }
                else
                {
                    view.toRegularDays = attendobj.ToRegularDays;
                    view.leavedays = attendobj.LeaveDays;
                    view.LeaveDeductions = msrmanage.GetLeaveDeductions(view.Id, one, view.PerformanceSalary, attendobj.DeserveToRegularDays, view.leavedays);
                    view.TardyWithhold = attendobj.TardyWithhold;
                    view.LeaveWithhold = attendobj.LeaveWithhold;
                }


                view.OvertimeCharges = item.OvertimeCharges;
                view.Bonus = item.Bonus;

               
                view.OtherDeductions = item.OtherDeductions;
               
                #region 应发工资1赋值
                view.SalaryTwo =msrmanage.GetSalarytwo(view.SalaryOne, view.OvertimeCharges,view.Bonus,view.LeaveDeductions,view.TardyWithhold,view.LeaveWithhold,view.OtherDeductions);
                #endregion
                item.PersonalSocialSecurity = eseobj.PersonalSocialSecurity;
                view.PersonalSocialSecurity = item.PersonalSocialSecurity;
                view.PersonalIncomeTax = eseobj.PersonalIncomeTax;
                item.Total=msrmanage.GetTotal(view.Id,view.SalaryTwo,view.PersonalSocialSecurity,view.PersonalIncomeTax);
                view.Total = item.Total;
                view.PayCardSalary = msrmanage.GetPaycardSalary(view.Id,view.Total,view.PersonalSocialSecurity,eseobj.ContributionBase);
                view.CashSalary =msrmanage.GetCashSalary(view.Id,view.Total,view.PayCardSalary);
                result.Add(view);
            }
            
             var newobj = new
            {
                code = 0,
                msg = "",
                count = eselist.Count(),
                data = result
            };
            return Json(newobj, JsonRequestBehavior.AllowGet);
        }

       /// <summary>
       /// 年月份改变
       /// </summary>
       /// <returns></returns>
        public ActionResult UpdateTime() {
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资
            var time = msrmanage.GetList().Where(s => s.IsDel == false).FirstOrDefault().YearAndMonth;
            string mytime = DateTime.Parse(time.ToString()).Year + "-" + DateTime.Parse(time.ToString()).Month;
            ViewBag.time = mytime;
            return View();
        }
        [HttpPost]
        public ActionResult UpdateTime(string CurrentTime) {
            var AjaxResultxx = new AjaxResult();
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资
            try
            {
                var msrlist=msrmanage.GetList().Where(s=>s.IsDel==false).ToList();
                for (int i = 0; i < msrlist.Count(); i++)
                {
                    msrlist[i].YearAndMonth =Convert.ToDateTime(CurrentTime);
                    msrmanage.Update(msrlist[i]);
                    AjaxResultxx = msrmanage.Success();
                }
            }
            catch (Exception ex)
            {
               AjaxResultxx= msrmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx,JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteSalaryManageEmp(string list)
        {
            MonthlySalaryRecordManage msrmanage = new MonthlySalaryRecordManage();//员工月度工资
            EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
            AttendanceInfoManage admanage = new AttendanceInfoManage();
            MeritsCheckManage mcmanage = new MeritsCheckManage();
            var AjaxResultxx = new AjaxResult();
            try
            {
                string[] ids = list.Split(',');
                for (int i = 0; i < ids.Length - 1; i++)
                {
                    string id = ids[i];
                    var ad = msrmanage.GetEntity(int.Parse(id));
                  AjaxResultxx.Success=  msrmanage.EditEmpMS(ad.EmployeeId);
                    if (AjaxResultxx.Success) {
                        bool e = esemanage.EditEmpSalaryState(ad.EmployeeId);//员工体系表禁用该员工
                        AjaxResultxx.Success = e;
                    }
                    if (AjaxResultxx.Success)
                    {
                        bool a = admanage.EditEmpStateToAds(ad.EmployeeId);//员工考勤表禁用该员工
                        AjaxResultxx.Success = a;
                    }
                    if (AjaxResultxx.Success)
                    {
                        bool e = mcmanage.EditEmpStateToMC(ad.EmployeeId);//员工绩效表禁用该员工
                        AjaxResultxx.Success = e;
                    }
                }
            }
            catch (Exception ex)
            {
                AjaxResultxx = msrmanage.Error(ex.Message);
            }
            return Json(AjaxResultxx, JsonRequestBehavior.AllowGet);
        }

    }
}