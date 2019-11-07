using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 本科成绩查询基本信息
    /// </summary>
   public class EssentialinformationEnucView
    {
        /// <summary>
        /// 考藉号
        /// </summary>
        public string PassNumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string identitydocument { get; set; }
        /// <summary>
        /// 专业
        /// </summary>
        public string MajorID { get; set; }
    }
}
