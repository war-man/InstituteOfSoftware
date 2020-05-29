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
    using SiliconValley.InformationSystem.Business.Common;

    public class EmpTransactionManage:BaseBusiness<EmpTransaction>
    {
      
        /// <summary>
        /// 获取异动表中离职的员工
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public EmpTransaction GetDelEmp(string empid) {
            //获取离职异动的id
            var delid = GetTypeidByTname("离职").ID ;
            var etlist = this.GetList().Where(s => s.EmployeeId == empid && s.TransactionType==delid).FirstOrDefault();
            return etlist;
        }
        /// <summary>
        /// 根据异动名称获取其异动类型对象
        /// </summary>
        /// <param name="tname"></param>
        /// <returns></returns>
        public MoveType GetTypeidByTname(string tname) {
            MoveTypeManage mtmanage = new MoveTypeManage();
            return mtmanage.GetList().Where(s => s.MoveTypeName == tname).FirstOrDefault();
        }

       
    }
}
