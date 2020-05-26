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

        /// <summary>
        /// 员工信息录入包含转正时间时添加该员工的异动数据
        /// </summary>
        /// <param name="empid"></param>
        /// <param name="positivetime"></param>
        /// <returns></returns>
        public bool InsertETSData(string empid,DateTime positivetime) {
            bool result = false;
            try
            {
                EmpTransaction ets = new EmpTransaction();
                ets.EmployeeId = empid;
                ets.TransactionTime = positivetime;
                ets.TransactionType = GetTypeidByTname("转正").ID;
                this.Insert(ets);
                result = true;
                BusHelper.WriteSysLog("异动表转正异动数据添加成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {
                result = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
           
            return result;
        }
    }
}
