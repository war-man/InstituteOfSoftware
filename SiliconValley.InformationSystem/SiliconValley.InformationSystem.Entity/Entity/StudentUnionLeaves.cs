using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.MyEntity
{
    /// <summary>
    /// 学生会成员撤销表
    /// </summary>
    [Table("StudentUnionLeaves")]
    public class StudentUnionLeaves
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 学生会表id
        /// </summary>
        public int Union_id { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; }
        /// <summary>
        /// 撤销时间
        /// </summary>
        public DateTime Datetimes { get; set; }
    }
}
