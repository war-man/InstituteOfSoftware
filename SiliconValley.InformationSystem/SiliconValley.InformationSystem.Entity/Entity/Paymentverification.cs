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
    /// 核对缴费是否成功
    /// </summary>
    [Table("Paymentverification")]
  public  class Paymentverification
    {
        [Key]
       public int id { get; set; }
        /// <summary>
        /// 是否成功，null为待审核 1通过，2作废，3撤销
        /// </summary>
       public string Passornot { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
       public string OddNumbers { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
       public DateTime? AddDate { get; set; }
        /// <summary>
        /// 收款方式
        /// </summary>
       public string Paymentmethod { get; set; }
    }
}
