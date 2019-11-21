using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 栋的页面model
    /// </summary>
   public class TungView
    {
    
        /// <summary>
        /// 栋名称
        /// </summary>
        public string TungName { get; set; }
        /// <summary>
        /// 栋地址
        /// </summary>
        public string TungAddress { get; set; }

        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// 居住人数
        /// </summary>
        public int personcount { get; set; }
    }
}
