using SiliconValley.InformationSystem.Business.Common;
using SiliconValley.InformationSystem.Business.Employment;
using SiliconValley.InformationSystem.Entity.Base_SysManage;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Channel
{
    /// <summary>
    /// 渠道员工的业务类
    /// </summary>
    public class ChannelStaffBusiness : BaseBusiness<ChannelStaff>
    {
        EmploymentStaffBusiness dbempstaff = new EmploymentStaffBusiness();
        /// <summary>
        /// 得到没有离职的渠道专员
        /// </summary>
        /// <returns>List<ChannelStaff></returns>
        public List<ChannelStaff> GetChannelStaffs()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据这个渠道专员id获取渠道专员对象
        /// </summary>
        /// <param name="ChanneID"></param>
        /// <returns>ChannelStaff</returns>
        public ChannelStaff GetChannelByID(int ChanneID) {
          return  this.GetChannelStaffs().Where(a => a.ID == ChanneID).FirstOrDefault();
        }
        /// <summary>
        /// 根据员工id获取渠道专员对象
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public ChannelStaff GetChannelByEmpID(string empid) {
           return this.GetChannelStaffs().Where(a => a.EmployeesInfomation_Id == empid).FirstOrDefault();
        }
        
        /// <summary>
        /// 删除渠道员工
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool DelChannelStaff(string empid) {
            ChannelStaff channelStaff=  this.GetChannelByEmpID(empid);
            channelStaff.IsDel = true;
            bool result = false;
            try
            {
                this.Update(channelStaff);
                result = true;
                BusHelper.WriteSysLog("当渠道员工离职的时候，对渠道员工的isdel进行修改，位于Channel文件夹中ClassesBusiness业务类中DelChannelStaff方法，编辑成功。", EnumType.LogType.编辑数据);
            }
            catch (Exception ex)
            {

                result = false;
                BusHelper.WriteSysLog("当渠道员工离职的时候，对渠道员工的isdel进行修改，位于Channel文件夹中ClassesBusiness业务类中DelChannelStaff方法，编辑失败。", EnumType.LogType.编辑数据);
            }
            return result;
        }
        /// <summary>
        /// 添加渠道员工
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public bool AddChannelStaff(string empid)
        {
            ChannelStaff channelStaff = new ChannelStaff();
            channelStaff.IsDel = false;
            channelStaff.EmployeesInfomation_Id = empid;
            bool result = false;
            try
            {
                this.Insert(channelStaff);
                result = true;
                BusHelper.WriteSysLog("当添加渠道员工的时候，位于Channel文件夹中ClassesBusiness业务类中AddChannelStaff方法，添加成功。", EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {

                result = false;
                BusHelper.WriteSysLog("当添加渠道员工的时候，位于Channel文件夹中ClassesBusiness业务类中AddChannelStaff方法，添加失败。", EnumType.LogType.添加数据);
            }
            return result;
        }
    }
}
