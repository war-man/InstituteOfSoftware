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
    }
}
