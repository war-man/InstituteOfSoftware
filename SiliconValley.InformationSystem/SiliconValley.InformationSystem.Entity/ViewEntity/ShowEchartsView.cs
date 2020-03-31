using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 统计市场老师备案数量、上门量、报名量
    /// </summary>
   public class ShowEchartsView
    {
        /// <summary>
        /// 备案数量
        /// </summary>
        public int KeepOnRecordCount { get; set; }

        /// <summary>
        /// 上门量
        /// </summary>
        public int GoSchoolCount { get; set; }
        /// <summary>
        /// 报名量
        /// </summary>
        public int SignUpCount { get; set; }
    }
}
