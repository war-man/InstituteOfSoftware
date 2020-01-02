using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EmpTransactionBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;
    using SiliconValley.InformationSystem.Business.SchoolAttendanceManagementBusiness;
   public class EmpTransactionManage:BaseBusiness<EmpTransaction>
    {
      
        /// <summary>
        /// 获取异动表中离职的员工
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public EmpTransaction GetDelEmp(string empid) {
            EmpTransactionManage etmanage = new EmpTransactionManage();
            MoveTypeManage mtmanage = new MoveTypeManage();
            //获取离职异动的id
            var delid = mtmanage.GetList().Where(s => s.MoveTypeName == "离职").FirstOrDefault().ID;
            var etlist = etmanage.GetList().Where(s => s.EmployeeId == empid && s.TransactionType==delid).FirstOrDefault();
            return etlist;
        } 

    }
}
