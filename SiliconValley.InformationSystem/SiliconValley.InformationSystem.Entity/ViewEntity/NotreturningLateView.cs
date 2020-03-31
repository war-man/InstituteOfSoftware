using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 晚归登记
    /// </summary>
  public  class NotreturningLateView
    {
        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 学生编号
        /// </summary>
        public string StudentNumber { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }

        /// <summary>
        /// 监察员
        /// </summary>
        public string InspectorName { get; set; }

        /// <summary>
        /// 当前学生的班主任
        /// </summary>
        public string HeadMasterName { get; set; }
    }
}
