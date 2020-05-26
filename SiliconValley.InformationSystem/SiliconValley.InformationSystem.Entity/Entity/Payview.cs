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
    /// 审核缴费表
    /// </summary>
    [Table("Payview")]
  public  class Payview
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudenID { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public Nullable<decimal> Amountofmoney { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public Nullable<int> FinanceModelid { get; set; }
        /// <summary>
        /// 消费时间
        /// </summary>
        public Nullable<System.DateTime> AddDate { get; set; }
        /// <summary>
        /// 名目
        /// </summary>
        public int Costitemsid { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public Nullable<bool> IsDelete { get; set; }
    }
}
