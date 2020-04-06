using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    /// <summary>
    /// 内训费用
    /// </summary>
    public class InternalTrainingCost
    {
        public int ID { get; set; }

        /// <summary>
        /// 阶段
        /// </summary>
        public int grand { get; set; }

        /// <summary>
        /// 费用
        /// </summary>
        public float cost { get; set; }

        public bool IsUsing { get; set; }
    }
}
