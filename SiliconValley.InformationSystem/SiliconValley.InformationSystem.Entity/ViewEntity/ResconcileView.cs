using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 排课视图
    /// </summary>
   public class ResconcileView
    {
        /// <summary>
        /// 排课编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 教室编号
        /// </summary>
        public Classroom ClassRoom_Id { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string Curriculum_Id { get; set; }
        /// <summary>
        /// 课程时间字段
        /// </summary>
        public string Curse_Id { get; set; }
        /// <summary>
        /// 班级编号
        /// </summary>
        public ClassSchedule ClassSchedule_Id { get; set; }
        /// <summary>
        /// 系统创建时间
        /// </summary>
        public Nullable<System.DateTime> NewDate { get; set; }
        /// <summary>
        /// 其他说明
        /// </summary>
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
