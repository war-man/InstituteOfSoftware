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
    /// 床位数量表
    /// </summary>
    [Table(name: "BenNumber")]
    public class BenNumber
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 床位号
        /// </summary>
        public string BenNo { get; set; }

        /// <summary>
        /// false可用，true 不可用
        /// </summary>
        public bool IsDelete { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
