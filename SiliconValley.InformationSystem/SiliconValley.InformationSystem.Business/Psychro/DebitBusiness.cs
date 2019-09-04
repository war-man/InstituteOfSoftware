using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Psychro
{
    /// <summary>
    /// 员工借资页面
    /// </summary>
    public class DebitBusiness : BaseBusiness<Debit>
    {
        /// <summary>
        /// 获取所有的成功借资数据
        /// </summary>
        /// <returns></returns>
        public List<Debit> GetAll() {
          return  this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据员工编号获取这个员工的借资记录
        /// </summary>
        /// <param name="EmpID"></param>
        /// <returns></returns>
        public List<Debit> GetDebitsByEmpID( string EmpID) {
            return this.GetAll().Where(a => a.EmpNumber == EmpID).ToList();
        }
    }
}
