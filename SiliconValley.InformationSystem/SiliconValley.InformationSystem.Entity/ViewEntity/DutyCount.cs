using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{

    using SiliconValley.InformationSystem.Entity.MyEntity;

    /// <summary>
    /// 值班次数
    /// </summary>
    public class DutyCount
    {
        /// <summary>
        /// 值班类型
        /// </summary>
        public BeOnDuty DutyType { get; set; }

        /// <summary>
        /// 值班次数
        /// </summary>
        public float Count { get; set; } 
    }
}
