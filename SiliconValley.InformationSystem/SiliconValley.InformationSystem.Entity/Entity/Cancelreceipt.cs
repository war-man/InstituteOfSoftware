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
    ///财务入账撤销表
    /// </summary>
    [Table("Cancelreceipt")]
   public class Cancelreceipt
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 入账表id
        /// </summary>
        public int Paymentverid { get; set; }
        /// <summary>
        /// 缴费表id
        /// </summary>
        public int FeeRecordid { get; set; }
    }
}
