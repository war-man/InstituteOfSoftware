using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView
{
   public class DistributionView
    {
        /// <summary>
        /// 班级名字
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 专业名字
        /// </summary>
        public string SpecialtyName { get; set; }

        /// <summary>
        /// 阶段名字
        /// </summary>
        public string GrandName { get; set; }

        /// <summary>
        /// 班级人数
        /// </summary>
        public int StudentCount { get; set; }

        /// <summary>
        /// 班主任名字
        /// </summary>
        public string HeadmasterName { get; set; }

        /// <summary>
        /// 班主任电话
        /// </summary>
        public string HeadmasterPhone { get; set; }

        /// <summary>
        /// 教员名字
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 教员电话
        /// </summary>
        public string TeacherPhone { get; set; }
    }
}
