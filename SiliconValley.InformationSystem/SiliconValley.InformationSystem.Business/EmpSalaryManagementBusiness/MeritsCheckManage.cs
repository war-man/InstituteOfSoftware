using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.MyEntity;
namespace SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness
{
   public class MeritsCheckManage:BaseBusiness<MeritsCheck>
    {
        /// <summary>
        /// 往员工绩效考核表加入员工编号
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool AddEmpToMeritsCheck(string empid)
        {
            bool result = false;
            try
            {
                MeritsCheck ese = new MeritsCheck();
                ese.EmployeeId = empid;
                ese.IsDel = false;
                 ese.YearAndMonth=this.GetList().LastOrDefault().YearAndMonth;
                this.Insert(ese);
                result = true;
                BusHelper.WriteSysLog("绩效考核表添加员工成功", Entity.Base_SysManage.EnumType.LogType.添加数据);

            }
            catch (Exception ex)
            {
                result = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return result;

        }

        /// <summary>
        /// 往员工绩效考核表加入员工编号
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool EditEmpStateToMC(string empid)
        {
            bool result = false;
            try
            {
               var mc= this.GetList().Where(e => e.EmployeeId == empid).FirstOrDefault();
                mc.IsDel = true;
                this.Update(mc);
                result = true;
                BusHelper.WriteSysLog("绩效考核表禁用员工成功", Entity.Base_SysManage.EnumType.LogType.编辑数据);

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
