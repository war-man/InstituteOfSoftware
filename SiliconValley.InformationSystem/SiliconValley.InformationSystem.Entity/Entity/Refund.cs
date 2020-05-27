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
    /// 预入费退费
    /// </summary>
    [Table("Refund")]
   public class Refund
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 预入费id
        /// </summary>
        public int Preentid { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Reason { get; set; } 
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
