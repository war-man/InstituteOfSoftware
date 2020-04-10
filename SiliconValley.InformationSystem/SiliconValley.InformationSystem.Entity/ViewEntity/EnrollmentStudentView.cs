using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 本科学员模型表
    /// </summary>
   public class EnrollmentStudentView
    {
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentNumber { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 身份证
        /// </summary>
        public string identitydocument { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 班主任
        /// </summary>
        public string Headmasters { get; set; }
        public string School { get; set; }
        public string Registeredbatch { get; set; }
        public string PassNumber { get; set; }
        public string MajorID { get; set; }
        public int Alreadypassed { get; set; }
    }
}
