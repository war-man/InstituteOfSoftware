using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    /// <summary>
    /// 班级上的课程类视图
    /// </summary>
   public class ClassCourseView
    {
        public int ID { get; set; }
        public EmployeesInfo Teacher { get; set; }
        /// <summary>
        /// 课程id
        /// </summary>
        public Curriculum Skill { get; set; }
        public ClassSchedule ClassNumber { get; set; }
        public Nullable<bool> IsDel { get; set; }

        public DateTime BeginDate { get; set; }

        public Nullable<DateTime> EndDate { get; set; }
    }
}
