using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 该实体用于存储咨询师的转化率
    /// </summary>
   public class ZhuanghuanluData
    {
        /// <summary>
        /// 咨询师名称
        /// </summary>
        public string TeacherName
        {
            get;set;
        }
        /// <summary>
        /// 转化率
        /// </summary>
        public int Number
        {
            get;set;
        }
    }
}
