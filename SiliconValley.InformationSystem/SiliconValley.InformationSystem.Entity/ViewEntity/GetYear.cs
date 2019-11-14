using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 存储某年的单休月份
    /// </summary>
   public class GetYear
    {
        /// <summary>
        /// 年份
        /// </summary>
        public string YearName { get; set; }
        /// <summary>
        /// 开始月份
        /// </summary>
        public int StartmonthName { get; set; }
        /// <summary>
        /// 结束月份
        /// </summary>
        public int EndmonthName { get; set; }
    }
}
