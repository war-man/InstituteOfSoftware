using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    /// <summary>
    /// 审核缴费信息对应表
    /// </summary>
    [Table("PayviewPaymentver")]
  public  class PayviewPaymentver
    {
        public int id { get; set; }
        /// <summary>
        /// 缴费审核
        /// </summary>
        public int Payviewid { get; set; }
        /// <summary>
        /// 核对审核
        /// </summary>
        public int Paymentver { get; set; }
    }
}
