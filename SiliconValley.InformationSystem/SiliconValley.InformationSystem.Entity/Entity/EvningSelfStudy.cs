using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table("EvningSelfStudy")]
    public class EvningSelfStudy
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 班级编号
        /// </summary>
        public int ClassSchedule_id { get; set; }
        /// <summary>
        /// 教室编号
        /// </summary>
        public int Classroom_id { get; set; }
        /// <summary>
        /// 上课时间段
        /// </summary>
        public string curd_name { get; set; }
        /// <summary>
        /// 排课时间
        /// </summary>
        public DateTime Anpaidate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Newdate { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string emp_id { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Rmark { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
