using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Util;

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

        public bool EditEmpMS(string empid)
        {
            var ems = this.GetEntity(empid);
            bool result = false;
            try
            {
                ems.IsDel = true;
                this.Update(ems);
                result = true;
                BusHelper.WriteSysLog("月度工资表去除该员工", Entity.Base_SysManage.EnumType.LogType.编辑数据);
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
        public AttendanceInfo GetAttendanceInfoByEmpid(string empid,DateTime time)
        {
            AttendanceInfoManage attmanage = new AttendanceInfoManage();
            var att = attmanage.GetList().Where(s => s.EmployeeId == empid && s.YearAndMonth==time).FirstOrDefault();
            return att;
        }


        /// <summary>
        /// 根据员工编号获取绩效考核表中的该员工对象
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public MeritsCheck GetMCByEmpid(string empid,DateTime time)
        {
            MeritsCheckManage mcmanage = new MeritsCheckManage();
            var mcobj = mcmanage.GetList().Where(s => s.EmployeeId == empid && s.YearAndMonth==time).FirstOrDefault();
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
        public decimal? GetSalaryone(decimal? one,decimal? PerformanceSalary,decimal? netbookSubsidy,decimal? socialSecuritySubsidy)
        {
            decimal ?SalaryOne=one;
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
            AjaxResult result=new AjaxResult();
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
                else {
                    countsalary = null;
                }          
               
            }
            catch (Exception ex)
            {
               result= this.Error(ex.Message);
                countsalary = null;
            }
           
                return countsalary;
        }



    }
}
