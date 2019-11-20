using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.EmployeesBusiness;
namespace SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness
{
   

  public  class EmplSalaryEmbodyManage:BaseBusiness<EmplSalaryEmbody>
    {
        /// <summary>
        /// 往员工工资体系表加入员工编号
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool AddEmpToEmpSalary(string empid) {
            bool result = false;
            try
            {
                EmplSalaryEmbody ese = new EmplSalaryEmbody();               
                ese.EmployeeId = empid;

                EmployeesInfoManage empmanage = new EmployeesInfoManage();
                var emp=empmanage.GetEntity(empid);
                ese.BaseSalary = 2000;
                ese.PerformancePay = 0;
                if (emp.ProbationSalary != null)
                {
                    ese.PositionSalary = emp.ProbationSalary - ese.BaseSalary - ese.PerformancePay;
                }
                else {
                    ese.PositionSalary = emp.Salary - ese.BaseSalary - ese.PerformancePay;
                }

                ese.IsDel = false;
                this.Insert(ese);
                result = true;
                BusHelper.WriteSysLog("工资体系表添加员工成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
                
            }
            catch (Exception ex)
            {
                result = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return result;
           
        }



        public bool EditEmpSalaryState(string empid) {
            var ese=this.GetEntity(empid);
            bool result = false;
            try
            {
                ese.IsDel = true;
                this.Update(ese);
                result = true;
                BusHelper.WriteSysLog("工资体系表去除该员工", Entity.Base_SysManage.EnumType.LogType.编辑数据);
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
