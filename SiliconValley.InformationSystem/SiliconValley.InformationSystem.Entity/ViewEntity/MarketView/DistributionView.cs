using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.MarketView
{
    /// <summary>
    /// 用于执行分配，post 请求中的参数
    /// </summary>
    public class DistributionView
    {
        /// <summary>
        ///上级id
        /// </summary>
        public int? RegionalDirectorID { get; set; }


        /// <summary>
        /// 当前被分配的员工id
        /// </summary>
        public int ChannelStaffID { get; set; }

        /// <summary>
        /// 分配的区域id
        /// </summary>
        public string RegionIDs { get; set; }
    }
}
