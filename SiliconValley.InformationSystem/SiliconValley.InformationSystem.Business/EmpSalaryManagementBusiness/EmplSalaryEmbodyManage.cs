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
                if (emp.PositiveDate == emp.EntryTime)
                {
                    if (empmanage.GetPositionByEmpid(empid).PositionName.Contains("主任"))
                    {
                        ese.PerformancePay = 1000;
                    }
                    else
                    {
                        ese.PerformancePay = 500;
                    }
                    ese.PositionSalary = emp.Salary - ese.BaseSalary - ese.PerformancePay;
                }
                else {
                    ese.PerformancePay = null;
                    ese.PositionSalary = emp.ProbationSalary - ese.BaseSalary - ese.PerformancePay;
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


        /// <summary>
        /// 禁用某员工
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool EditEmpSalaryState(string empid) {
            var ese = this.GetList().Where(e => e.EmployeeId == empid).FirstOrDefault() ;
            bool result = false;
            try
            {
                ese.IsDel = true;
                this.Update(ese);
                result = true;
                BusHelper.WriteSysLog("工资体系表禁用该员工成功！", Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            catch (Exception ex)
            {
                result = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.编辑数据);
            }
            return result;
        }


        /// <summary>
        /// 根据员工编号获取该员工工资体系对象
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public EmplSalaryEmbody GetEseByEmpid(string empid) {
           var ese= this.GetList().Where(s => s.EmployeeId == empid).FirstOrDefault();
            return ese;
        }
    }
}
