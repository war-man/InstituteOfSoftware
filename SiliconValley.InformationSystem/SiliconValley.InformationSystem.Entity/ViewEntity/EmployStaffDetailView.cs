using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    public class EmployStaffDetailView
    {
        /// <summary>
        /// 就业专员id
        /// </summary>
        public int EmployStaffID { get; set; }

        /// <summary>
        /// 员工
        /// </summary>
        public EmployeesInfo emp { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public Department Department { get; set; }
        /// <summary>
        /// 岗位
        /// </summary>
        public Position Position { get; set; }
        /// <summary>
        /// 就业区域对象
        /// </summary>
        public EmploymentAreas Areas { get; set; }
        /// <summary>
        ///专员正在带的班级列表
        /// </summary>
        public List<ClassSchedule> ClassSchedulesing { get; set; }
        /// <summary>
        ///就业专员已毕业班级
        /// </summary>
        public List<ClassSchedule> ClassSchedulesed { get; set; }
        /// <summary>
        /// 带班经验
        /// </summary>
        public string AttendClassStyle { get; set; }
        /// <summary>
        /// 就业经验
        /// </summary>
        public string EmployExperience { get; set; }
        /// <summary>
        /// 工作经验
        /// </summary>
        public string WorkExperience { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark {get;set;}

    }
}
