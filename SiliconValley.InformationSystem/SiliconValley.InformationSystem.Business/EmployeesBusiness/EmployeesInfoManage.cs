using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EmployeesBusiness
{
    using Base_SysManage;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    /// <summary>
    /// 员工业务类
    /// </summary>
    public class EmployeesInfoManage:BaseBusiness<EmployeesInfo>
    {
        /// <summary>
        /// 获取所有没有离职的员工
        /// </summary>
        /// <returns></returns>
        public List<EmployeesInfo> GetAll() {
          return  this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据员工编号获取员工对象
        /// </summary>
        /// <param name="empinfoid">员工编号</param>
        /// <returns></returns>
        public EmployeesInfo GetInfoByEmpID(string empinfoid) {
           return this.GetAll().Where(a => a.EmployeeId == empinfoid).FirstOrDefault();
        }

        /// <summary>
        /// 添加员工借资
        /// </summary>
        /// <param name="debit"></param>
        /// <returns></returns>
        public bool Borrowmoney(Debit debit) {
            BaseBusiness<Debit> dbdebit = new BaseBusiness<Debit>();
            bool result = false;
            try
            {
                dbdebit.Insert(debit);
                result = true;
            }
            catch (Exception)
            {

            }
            return result;
        }
    }
}
