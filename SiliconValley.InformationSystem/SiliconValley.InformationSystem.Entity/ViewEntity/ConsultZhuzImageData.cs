using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 月度咨询师分量完成量
    /// </summary>
   public class ConsultZhuzImageData
    {
        /// <summary>
        /// 月份
        /// </summary>
        public int MonthName
        {
            get;
            set;
        }
        /// <summary>
        /// 完成数量
        /// </summary>
        public int wanchengcount
        {
            get;set;
        }
        /// <summary>
        /// 未完成数量
        /// </summary>
        public int nowanchengcount
        {
            get;set;
        }
        /// <summary>
        /// 分量数量
        /// </summary>
        public int fengliangarry
        {
            get;set;
        }
    }
}
