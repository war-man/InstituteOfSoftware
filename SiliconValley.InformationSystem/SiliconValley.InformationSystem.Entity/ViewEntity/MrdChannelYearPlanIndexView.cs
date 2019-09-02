using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于显示渠道专员年度计划数据
    /// </summary>
    public class MrdChannelYearPlanIndexView
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmpStaffID { get; set; }
        /// <summary>
        /// 年度渠道专员计划id
        /// </summary>
        public Nullable<int> ChannelYearPlanID { get; set; }
        /// <summary>
        /// 渠道专员id
        /// </summary>
        public int ChannelStaffID { get; set; }

        /// <summary>
        /// 专员名字
        /// </summary>
        public string EmpName { get; set; }
        /// <summary>
        /// 专员电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 专员计划量
        /// </summary>
        public int PlanNumber { get; set; }
        /// <summary>
        /// 实际数量
        /// </summary>
        public int SignUpNumber { get; set; }
        /// <summary>
        /// 备案数量
        /// </summary>
        public int BeianNumber { get; set; }
        /// <summary>
        /// 上门量
        /// </summary>
        public int GoSchoolNumber { get; set; }
        /// <summary>
        /// 区域id
        /// </summary>
        public int RegionID { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string Region { get; set; }
        /// <summary>
        /// 入职时间
        /// </summary>
        public System.DateTime ChannelDate { get; set; }
        /// <summary>
        /// 离职时间
        /// </summary>
        public Nullable<System.DateTime> QuitDate { get; set; }
        /// <summary>
        /// 借资次数
        /// </summary>
        public Nullable<int> DebitNumber { get; set; }
    }
}
