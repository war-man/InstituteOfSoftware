using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.MyEntity;
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
                this.Update(ems);
                result = true;
                BusHelper.WriteSysLog("月度工资表编辑员工离职成功", Entity.Base_SysManage.EnumType.LogType.编辑数据);
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
        public AttendanceInfo GetAttendanceInfoByEmpid(string empid)
        {
            AttendanceInfoManage attmanage = new AttendanceInfoManage();
            var att = attmanage.GetList().Where(s => s.EmployeeId == empid).FirstOrDefault();
            return att;
        }


        /// <summary>
        /// 根据员工编号获取绩效考核表中的该员工对象
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public MeritsCheck GetMCByEmpid(string empid)
        {
            MeritsCheckManage mcmanage = new MeritsCheckManage();
            var mcobj = mcmanage.GetList().Where(s => s.EmployeeId == empid).FirstOrDefault();
            return mcobj;
        }
    }
}
