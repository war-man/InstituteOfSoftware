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
    /// 学费退费表
    /// </summary>
    [Table("Tuitionrefund")]
   public class Tuitionrefund
    {
        [Key]
       public int id { get; set; }
        /// <summary>
        /// 退费明目
        /// </summary>
       public int StudentFeeRecordId { get; set; }
        /// <summary>
        /// 退费金额
        /// </summary>
      public decimal Amountofmoney { get; set; }
        /// <summary>
        /// 操作人工号
        /// </summary>
      public string Empnumber { get; set; }
        /// <summary>
        /// 退费时间
        /// </summary>
      public DateTime AddDate { get; set; }
    }
}
