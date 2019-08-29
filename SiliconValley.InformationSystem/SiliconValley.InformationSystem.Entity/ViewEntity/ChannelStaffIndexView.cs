using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于显示渠道员工的类
    /// </summary>
    public class ChannelStaffIndexView
    {
        /// <summary>
        /// 渠道员工的id
        /// </summary>
        public int ChannelStaffID { get; set; }
        /// <summary>
        /// 员工id
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string EmpName { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public Nullable<System.DateTime> EntryTime { get; set; }
        /// <summary>
        /// 转正时间
        /// </summary>
        public Nullable<System.DateTime> PositiveDate { get; set; }
        /// <summary>
        /// 区域名字
        /// </summary>
        public string RegionName { get; set; }
        /// <summary>
        /// 所属区域id
        /// </summary>
        public string RegionID { get; set; }
        /// <summary>
        /// 区域主管
        /// </summary>
        public string RegionalDirectorEmpName { get; set; }
        /// <summary>
        /// 主管id
        /// </summary>
        public Nullable<int> RegionalDirectorID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
