using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 学生报考
    /// </summary>
  public  class EnrollmentView
    {
        public int id { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 班级
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 考藉号
        /// </summary>
        public string PassNumber { get; set; }
        /// <summary>
        /// 身份证号
        /// </summary>
        public string identitydocument { get; set; }

    }
}
