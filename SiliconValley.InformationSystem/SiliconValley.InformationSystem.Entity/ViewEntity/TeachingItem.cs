using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SiliconValley.InformationSystem.Entity.MyEntity;
namespace SiliconValley.InformationSystem.Entity.ViewEntity
{

    /// <summary>
    /// 上专业课的次数
    /// </summary>
   public class TeachingItem
    {
        
        /// <summary>
        /// 阶段
        /// </summary>
        public Grand grand { get; set; }

        /// <summary>
        /// 课时  4节课=1课时
        /// </summary>
        public int NodeNumber { get; set; }
    }
}
