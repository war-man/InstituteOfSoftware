using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class ResconcileView
    {
        public int Id { get; set; }
        public Classroom ClassRoom_Id { get; set; }
        public string Curriculum_Id { get; set; }
        /// <summary>
        /// 课程时间字段
        /// </summary>
        public string Curse_Id { get; set; }
        public string ClassSchedule_Id { get; set; }
        /// <summary>
        /// 系统创建时间
        /// </summary>
        public Nullable<System.DateTime> NewDate { get; set; }
        public string Rmark { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        /// <summary>
        /// 排课时间
        /// </summary>
        public Nullable<System.DateTime> AnPaiDate { get; set; }
        /// <summary>
        /// 老师编号
        /// </summary>
        public EmployeesInfo EmployeesInfo_Id { get; set; }
    }
}
