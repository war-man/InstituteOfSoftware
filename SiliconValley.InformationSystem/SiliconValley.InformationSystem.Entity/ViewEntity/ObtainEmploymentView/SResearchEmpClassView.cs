using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView
{
    /// <summary>
    /// 用于访谈
    /// </summary>
    public class SResearchEmpClassView
    {
        public int classid { get; set; }
        public string classnumber { get; set; }
        public bool isgraduation { get; set; }

        /// <summary>
        /// 访谈次数
        /// </summary>
        public int interviewcount { get; set; }

        /// <summary>
        /// 实际访谈人数
        /// </summary>
        public int peoplecount { get; set; }
        /// <summary>
        /// 应访谈人数
        /// </summary>
        public int totalnumber { get; set; }
        /// <summary>
        /// 重复访谈次数
        /// </summary>
        public int repeatedinterviews { get; set; }
    }
}
