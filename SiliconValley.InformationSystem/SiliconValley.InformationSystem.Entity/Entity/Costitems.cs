using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.MyEntity
{
    //小名目
    [Table("Costitems")]
  public  class Costitems
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 费用
        /// </summary>
        public decimal Amountofmoney { get; set; }
        /// <summary>
        /// 阶段
        /// </summary>
        public int? Grand_id { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 类型如（学费）
        /// </summary>
        public int Rategory { get; set; }
    }
}
