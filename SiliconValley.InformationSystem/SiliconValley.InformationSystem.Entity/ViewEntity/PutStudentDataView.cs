using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 备案id获取班级信息
    /// </summary>
  public  class PutStudentDataView
    {
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StuName { get; set; }
        /// <summary>
        /// 备案id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 班级id
        /// </summary>
        public int ClassID { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>
        public string className { get; set; }
        /// <summary>
        /// 班主任
        /// </summary>
        public string HearName { get; set; }
        /// <summary>
        ///教学老师
        /// </summary>
        public string TeacherName { get; set; }
        /// <summary>
        /// 阶段名称
        /// </summary>
        public string GrandName { get; set; }
        /// <summary>
        /// 专业名称
        /// </summary>
        public string SpeciName { get; set; }
    }
}
