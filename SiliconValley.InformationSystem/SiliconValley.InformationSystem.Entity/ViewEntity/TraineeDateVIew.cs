using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 培训人模型表
    /// </summary>
  public  class TraineeDateVIew
    {
        public int id { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmployeeId { get; set; }
        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmpName { get; set; }
        /// <summary>
        /// 员工部门id
        /// </summary>
        public int departmentID { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string department { get; set; }
        /// <summary>
        /// 阶段名称
        /// </summary>
        public string grade_Id { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string sex { get; set; }
    }
}
