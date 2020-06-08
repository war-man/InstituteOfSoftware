using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.NetClientRecordBusiness
{
    using SiliconValley.InformationSystem.Business.Common;
    using SiliconValley.InformationSystem.Entity.MyEntity;
   public  class NetClientRecordManage:BaseBusiness<NetClientRecord>
    {
        /// <summary>
        /// 通过备案id添加网咨跟踪信息
        /// </summary>
        /// <returns></returns>
        public bool AddNCRData(int id) {
            NetClientRecord ncr = new NetClientRecord();
            bool result = false;
            try
            {
                ncr.SPRId = id;
                ncr.IsDel = false;
                this.Insert(ncr);
                result = true;
                BusHelper.WriteSysLog("网咨学生回访信息添加成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            catch (Exception ex)
            {
                result = false;
                BusHelper.WriteSysLog(ex.Message, Entity.Base_SysManage.EnumType.LogType.添加数据);
            }
            return result;
        }
        public bool AddNCRData(List<StudentPutOnRecord> spridlist) {
            bool result = false;
            try
            {
                foreach (var item in spridlist)
                {
                    NetClientRecord ncr = new NetClientRecord();
                    ncr.SPRId = item.Id;
                    ncr.IsDel = false;
                    this.Insert(ncr);
                    result = true;
                    BusHelper.WriteSysLog("网咨学生回访信息添加成功", Entity.Base_SysManage.EnumType.LogType.添加数据);
                }
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
