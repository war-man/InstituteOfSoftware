using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 排课实体(没有外键)
    /// </summary>
    public class ReconView
    {
        /// <summary>
        /// 排课编号
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 班级名称
        /// </summary>
        public string class_name { get; set; }
        /// <summary>
        /// 教室名称
        /// </summary>
        public string classroom_name { get; set; }
        /// <summary>
        /// 课程名称
        /// </summary>
        public string currname { get; set; }
        /// <summary>
        /// 任课老师名称
        /// </summary>
        public string teachername { get; set; }
        /// <summary>
        /// 上课时间段
        /// </summary>
        public string curdname { get; set; }
        /// <summary>
        /// 安排的日期
        /// </summary>
        public string anpaidate { get; set; }
        /// <summary>
        /// 其他说明
        /// </summary>
        public string ramak { get; set; }
        /// <summary>
        /// 班级编号
        /// </summary>
        public int classId { get; set; }
    }   
}
