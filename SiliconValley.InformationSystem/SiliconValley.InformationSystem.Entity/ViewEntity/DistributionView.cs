using SiliconValley.InformationSystem.Entity.MyEntity;
using System.Collections.Generic;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    public class DistributionView
    {

        /// <summary>
        /// 班级编号
        /// </summary>
        public string ClassNumber { get; set; }

        /// <summary>
        /// 阶段名称
        /// </summary>
        public string GrandName { get; set; }
        /// <summary>
        /// 班级人数
        /// </summary>
        public int StudengCount { get; set; }
        /// <summary>
        /// 专业方向名称
        /// </summary>
        public string SpecialtyName { get; set; }

    }
}
