using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    ///  该类是用于安排阶段晚自习数据处理的
    /// </summary>
   public class GrandClassAnpaiEvningSelf
    {
        /// <summary>
        /// 安排好的晚自习数据
        /// </summary>
        public List<EvningSelfStudy> evnlist { set; get; }

        /// <summary>
        /// 未安排专业课的班级及日期
        /// </summary>
        public List<EmptyClass> emplist { get; set; }
    }
}
