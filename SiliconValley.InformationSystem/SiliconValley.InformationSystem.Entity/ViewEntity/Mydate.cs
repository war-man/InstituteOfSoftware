using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 这个类用于处理排课业务的
    /// </summary>
   public class Mydate
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StarTime
        {
            get;set;
        }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndTime
        {
            get;set;
        }
    }
}
