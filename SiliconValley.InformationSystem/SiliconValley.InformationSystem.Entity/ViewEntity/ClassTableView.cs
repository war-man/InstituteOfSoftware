using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 班级表数据模型实体
    /// </summary>
   public class ClassTableView
    {
        /// <summary>
        /// 班级id
        /// </summary>
        public int classid { get; set; }
        /// <summary>
        /// 班级编号
        /// </summary>
        public string ClassNumber { get; set; }
        /// <summary>
        /// 班级状态毕业班或者未毕业
        /// </summary>
        public Nullable<bool> ClassStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string ClassRemarks { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public Nullable<bool> IsDelete { get; set; }
        /// <summary>
        /// 专业名称
        /// </summary>
        public string MajorName { get; set; }
        /// <summary>
        /// 阶段名称
        /// </summary>
        public string GradeName { get; set; }

        public string Headmaster { get; set; }//班主任

        public string qqGroup { get; set; } //qq群号

        public int ClassSize { get; set; }//班级人数

    }
}
