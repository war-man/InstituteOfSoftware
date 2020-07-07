using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.TM_Data.MyViewEntity
{
    /// <summary>
    /// 排课视图实体
    /// </summary>
    public class ReconcileView
    {
        /// <summary>
        /// 排课编号
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 班级编号
        /// </summary>
        public int ClassSchedule_Id { get; set; }
        /// <summary>
        /// 班级数据
        /// </summary>
        public string ClassNumber { get; set; }
        /// <summary>
        /// 教室编号
        /// </summary>
        public int ClassRoom_Id { get; set; }
        /// <summary>
        /// 教室名称
        /// </summary>
        public string ClassroomName { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmployeesInfo_Id { get; set; }
        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmpName { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string Curriculum_Id { get; set; }
        /// <summary>
        /// 上课时间段
        /// </summary>
        public string Curse_Id { get; set; }
        /// <summary>
        /// 上课日期
        /// </summary>
        public DateTime AnpaiDate { get; set; }
        /// <summary>
        /// 生成日期
        /// </summary>
        public DateTime NewDate { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool Isdelete { get; set; }
        /// <summary>
        /// 其他说明
        /// </summary>
        public string Rmark { get; set; }
    }
}
