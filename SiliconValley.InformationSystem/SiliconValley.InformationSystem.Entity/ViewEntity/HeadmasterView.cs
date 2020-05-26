using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于处理班主任值班显示页面
    /// </summary>
   public class HeadmasterView
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 值班时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 值班人
        /// </summary>
        public string Teachers { get; set; }

        /// <summary>
        /// 值班类型
        /// </summary>
        public string Types { get; set; }
    }
}
