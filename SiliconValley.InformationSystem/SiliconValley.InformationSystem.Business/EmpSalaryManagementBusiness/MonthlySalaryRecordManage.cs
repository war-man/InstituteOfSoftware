using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;
using SiliconValley.InformationSystem.Entity.ViewEntity.SalaryView;
namespace SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness
{
    public class MonthlySalaryRecordManage : BaseBusiness<MonthlySalaryRecord>
    {
        /// <summary>
        /// 往员工月度工资表加入员工编号
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool AddEmpToEmpMonthSalary(string empid)
        {
            bool result = false;
            try
            {
                MonthlySalaryRecord ese = new MonthlySalaryRecord();
                ese.EmployeeId = empid;
                ese.IsDel = false;
                this.Insert(ese);
                result = true;
                BusHelper.WriteSysLog("月度工资表添加员工成功", Entity.Base_SysManage.EnumType.LogType.添加数据);

            }
            catch (Exception ex)
            {
                result = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return result;

        }

        /// <summary>
        /// 去除该员工
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool EditEmpMS(string empid)
        {
            var ems = this.GetList().Where(s=>s.EmployeeId==empid).FirstOrDefault();
            bool result = false;
            try
            {
                ems.IsDel = true;
                this.Update(ems);
                result = true;
                BusHelper.WriteSysLog("月度工资表禁用该员工", Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            catch (Exception ex)
            {
                result = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            return result;
        }

        /// <summary>
        /// 根据员工编号获取员工体系表中该员工对象
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public EmplSalaryEmbody GetEmpsalaryByEmpid(string empid)
        {
            EmplSalaryEmbodyManage esemanage = new EmplSalaryEmbodyManage();
            var ese = esemanage.GetList().Where(s => s.EmployeeId == empid).FirstOrDefault();
            return ese;
        }

        /// <summary>
        /// 根据员工编号获取考勤统计表中的该员工对象
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public AttendanceInfo GetAttendanceInfoByEmpid(string empid, DateTime time)
        {
            AttendanceInfoManage attmanage = new AttendanceInfoManage();
            var att = attmanage.GetList().Where(s => s.EmployeeId == empid && s.YearAndMonth == time).FirstOrDefault();
            return att;
        }


        /// <summary>
        /// 根据员工编号获取绩效考核表中的该员工对象
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public MeritsCheck GetMCByEmpid(string empid, DateTime time)
        {
            MeritsCheckManage mcmanage = new MeritsCheckManage();
            var mcobj = mcmanage.GetList().Where(s => s.EmployeeId == empid && s.YearAndMonth == time).FirstOrDefault();
            return mcobj;
        }

        /// <summary>
        /// 计算绩效工资
        /// </summary>
        /// <param name="finalGrade">绩效分</param>
        /// <param name="performancelimit">绩效额度</param>
        /// <returns></returns>
        public decimal? GetempPerformanceSalary(decimal? finalGrade, decimal? performancelimit)
        {
            decimal? result;
            if (finalGrade == 100)
            {
                result = performancelimit;
            }
            else if (finalGrade < 100)
            {
                result = performancelimit - performancelimit * (1 - finalGrade * (decimal)0.01);
            }
            else
            {
                result = performancelimit * (finalGrade * (decimal)0.01);
            }
            return result;
        }
        /// <summary>
        /// 计算应发工资1
        /// </summary>
        /// <param name="one">基本工资+岗位工资</param>
        /// <param name="PerformanceSalary">绩效工资</param>
        /// <param name="netbookSubsidy">笔记本补助</param>
        /// <param name="socialSecuritySubsidy">社保补贴</param>
        /// <returns></returns>
        public decimal? GetSalaryone(decimal? one, decimal? PerformanceSalary, decimal? netbookSubsidy, decimal? socialSecuritySubsidy)
        {
            decimal? SalaryOne = one;
            if (!string.IsNullOrEmpty(PerformanceSalary.ToString()))
            {
                SalaryOne = SalaryOne + PerformanceSalary;
            }
            if (!string.IsNullOrEmpty(netbookSubsidy.ToString()))
            {
                SalaryOne = SalaryOne + netbookSubsidy;
            }
            if (!string.IsNullOrEmpty(socialSecuritySubsidy.ToString()))
            {
                SalaryOne = SalaryOne + socialSecuritySubsidy;
            }

            return SalaryOne;
        }


        /// <summary>
        /// 计算请假扣款
        /// </summary>
        /// <param name="one">基本工资+岗位工资</param>
        /// <param name="persalary">绩效工资</param>
        /// <param name="shouldday">应出勤天数</param>
        /// <param name="leaveday">请假天数</param>
        /// <returns></returns>
        public decimal? GetLeaveDeductions(int id, decimal? one, decimal? persalary, decimal? shouldday, decimal? leaveday)
        {
            AjaxResult result = new AjaxResult();
            decimal? countsalary;
            var msr = this.GetEntity(id);
            try
            {
                if (!string.IsNullOrEmpty(persalary.ToString()))
                {
                    countsalary = one + persalary;
                }
                else
                {
                    countsalary = one;
                }
                if (!string.IsNullOrEmpty(leaveday.ToString()))
                {
                    countsalary = countsalary / shouldday * leaveday;

                    msr.LeaveDeductions = countsalary;
                    this.Update(msr);
                    result = this.Success();
                    countsalary = (decimal)Math.Round(Convert.ToDouble(countsalary), 2);
                }
                else
                {
                    countsalary = null;
                }

            }
            catch (Exception ex)
            {
                result = this.Error(ex.Message);
                countsalary = null;
            }

            return countsalary;
        }


        /// <summary>
        /// 计算应发工资2
        /// </summary>
        /// <param name="salaryone">应发工资1</param>
        /// <param name="OvertimeCharges">加班费用</param>
        /// <param name="Bonus">奖金/元</param>
        /// <param name="LeaveDeductions">请假扣款</param>
        /// <param name="TardyWithhold">迟到扣款</param>
        /// <param name="LeaveWithhold">早退扣款</param>
        /// <param name="OtherDeductions">其他扣款</param>
        /// <returns></returns>
        public decimal? GetSalarytwo(decimal? salaryone, decimal? OvertimeCharges, decimal? Bonus, decimal? LeaveDeductions, decimal? TardyWithhold, decimal? LeaveWithhold, decimal? OtherDeductions)
        {
            decimal? SalaryTwo = salaryone;
            if (!string.IsNullOrEmpty(OvertimeCharges.ToString()))
            {
                SalaryTwo = SalaryTwo + OvertimeCharges;
            }
            if (!string.IsNullOrEmpty(Bonus.ToString()))
            {
                SalaryTwo = SalaryTwo + Bonus;
            }
            if (!string.IsNullOrEmpty(LeaveDeductions.ToString()))
            {
                SalaryTwo = SalaryTwo - LeaveDeductions;
            }
            if (!string.IsNullOrEmpty(TardyWithhold.ToString()))
            {
                SalaryTwo = SalaryTwo - TardyWithhold;
            }
            if (!string.IsNullOrEmpty(LeaveWithhold.ToString()))
            {
                SalaryTwo = SalaryTwo - LeaveWithhold;
            }
            if (!string.IsNullOrEmpty(OtherDeductions.ToString()))
            {
                SalaryTwo = SalaryTwo - OtherDeductions;
            }

            return SalaryTwo;
        }

        /// <summary>
        /// 计算工资合计
        /// </summary>
        /// <param name="id">月度工资编号</param>
        /// <param name="salarytwo">应发工资2</param>
        /// <param name="PersonalSocialSecurity">个人社保</param>
        /// <param name="PersonalIncomeTax">个税</param>
        /// <returns></returns>
        public decimal? GetTotal(int id, decimal? salarytwo, decimal? PersonalSocialSecurity, decimal? PersonalIncomeTax)
        {
            AjaxResult result = new AjaxResult();
            decimal? Total = salarytwo;
            if (!string.IsNullOrEmpty(PersonalSocialSecurity.ToString()))
            {
                Total = Total - PersonalSocialSecurity;
            }
            if (!string.IsNullOrEmpty(PersonalIncomeTax.ToString()))
            {
                Total = Total - PersonalIncomeTax;
            }
            try
            {
                var msr = this.GetEntity(id);
                msr.Total = Total;
                this.Update(msr);
                result = this.Success();
            }
            catch (Exception ex)
            {
                result = this.Error(ex.Message);
            }
            return Total;
        }

        /// <summary>
        /// 计算工资卡实发工资
        /// </summary>
        /// <param name="id"></param>
        /// <param name="total">合计</param>
        /// <param name="PersonalSocialSecurity">个人社保</param>
        /// /// <param name="PersonalSocialSecurity">社保缴费基数</param>
        /// <returns></returns>
        public decimal? GetPaycardSalary(int id, decimal? total, decimal? PersonalSocialSecurity, decimal? ContributionBase)
        {
           

            decimal? paycardsalary = null;
            if (!string.IsNullOrEmpty(PersonalSocialSecurity.ToString()))
            {     bool result = false;
                try
                { 
                    var msr = this.GetEntity(id);
                    if (total <= ContributionBase)
                    {
                        paycardsalary = total;
                    }
                    else
                    {
                        paycardsalary = ContributionBase - PersonalSocialSecurity;
                    }
                    msr.PayCardSalary = paycardsalary;
                    this.Update(msr);
                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                }

            }
            else
            {
                paycardsalary = null;
            }
            return paycardsalary;
        }

        /// <summary>
        /// 计算现金实发工资
        /// </summary>
        /// <param name="id"></param>
        /// <param name="total">合计</param>
        /// <param name="PaycardSalary">工资卡工资</param>
        /// <returns></returns>
        public decimal? GetCashSalary(int id, decimal? total, decimal? PaycardSalary)
        {
            bool result=false;
            decimal? cash = 0;
           
                try
            {
                if (!string.IsNullOrEmpty(PaycardSalary.ToString()))
                {
                    var msr = this.GetEntity(id);
                    msr.CashSalary = total - PaycardSalary;
                    this.Update(msr);
                    result = true;
                    cash = msr.CashSalary;
                }           
            else {
                cash = total;
            }
            }
                catch (Exception ex)
                {
                result = false;
                cash = null;
                }
          

            return cash;
        }
    }
}
