using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    public class EmpClassView
    {
        /// <summary>
        /// 班级编号
        /// </summary>
        public string ClassNumber { get; set; }
        /// <summary>
        /// 就业专员id
        /// </summary>
        public Nullable<int> EntID { get; set; }
        /// <summary>
        /// 员工名字
        /// </summary>
        public string EmpName { get; set; }
        /// <summary>
        /// 员工电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 阶段名称
        /// </summary>
        public string GrandName { get; set; }
        /// <summary>
        /// 接班时间
        /// </summary>
        public Nullable<DateTime> empclassDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 专业方向名称
        /// </summary>
        public string SpecialtyName { get; set; }
    }
}
