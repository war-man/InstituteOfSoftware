using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Entity.MyEntity;
namespace SiliconValley.InformationSystem.Business.EmpSalaryManagementBusiness
{
    public class AttendanceInfoManage : BaseBusiness<AttendanceInfo>
    {
        /// <summary>
        /// 往员工考勤表加入员工编号
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool AddEmpToAttendanceInfo(string empid)
        {
            bool result = false;
            try
            {
                AttendanceInfo ese = new AttendanceInfo();
                ese.EmployeeId = empid;
                ese.IsDel = false;
                ese.YearAndMonth = this.GetList().LastOrDefault().YearAndMonth;
                this.Insert(ese);
                result = true;
                BusHelper.WriteSysLog("考勤表添加员工成功", Entity.Base_SysManage.EnumType.LogType.添加数据);

            }
            catch (Exception ex)
            {
                result = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return result;

        }

         /// <summary>
        /// 往员工考勤表加入员工编号
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool EditEmpStateToAds(string empid)
        {
            bool result = false;
            try
            {
                var ads= this.GetList().Where(e => e.EmployeeId == empid).FirstOrDefault();
                ads.IsDel = true;
                this.Update(ads);
                result = true;
                BusHelper.WriteSysLog("考勤表禁用员工成功", Entity.Base_SysManage.EnumType.LogType.编辑数据);

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
