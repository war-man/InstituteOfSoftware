using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.TM_Data
{
    /// <summary>
    /// 上课违规类型表
    /// </summary>
   [Table("Violationofdiscipline")]
   public class Violationofdiscipline
    {
        /// <summary>
        /// 编号
        /// </summary>
        [Key]
        public int Id { get; set; } 

        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
    }
}
