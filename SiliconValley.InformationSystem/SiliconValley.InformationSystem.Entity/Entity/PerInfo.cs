using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    /// <summary>
    /// 预资详细表
    /// </summary>
    [Table("PerInfo")]
    public  class PerInfo
    {
        /// <summary>
        /// 详细id
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 预资单ID
        /// </summary>
        public int PreID { get; set; }
        /// <summary>
        /// 学生编号
        /// </summary>
        public string StudentNumber { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
