using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 退费项目视图
    /// </summary>
    [Table(name: "Refunditemsview")]
   public class Refunditemsview
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 学生学号
        /// </summary>
        public string StudenID { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 学生性别
        /// </summary>
        public bool Sex { get; set; }
        /// <summary>
        /// 退款项目
        /// </summary>
        public string cosrName { get; set; }
        /// <summary>
        /// 项目阶段
        /// </summary>
        public string GrandName { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal Amountofmoney { get; set; }
        /// <summary>
        /// 退款日期
        /// </summary>
        public DateTime AddDate { get; set; }
    }
}
