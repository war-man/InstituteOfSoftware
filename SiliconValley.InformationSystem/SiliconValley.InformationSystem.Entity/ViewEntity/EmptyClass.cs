using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于存放没有安排专业课的班级
    /// </summary>
   public class EmptyClass
    {
        /// <summary>
        /// 班级名称
        /// </summary>
        public List<string> ClassName { get; set; } 
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime date { get; set; }
    }
}
