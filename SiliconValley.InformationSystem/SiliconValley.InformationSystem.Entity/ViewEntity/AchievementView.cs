using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 成绩模型数据
    /// </summary>
    public class AchievementView
    {
        /// <summary>
        /// 考期
        /// </summary>
        public string Examinationperiod { get; set; }
        /// <summary>
        /// 课程代码
        /// </summary>
        public string Coursecode { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string Coursetitle { get; set; }
        /// <summary>
        /// 成绩
        /// </summary>
        public int Fraction { get; set; }
    }
}
