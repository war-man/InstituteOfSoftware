using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.MyEntity
{
    [Table("CostitemsX")]
    //大费用名目表
   public class CostitemsX
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 名目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
       public DateTime AddDate { get; set; }
        /// <summary>
        /// url地址
        /// </summary>
        public string Url { get; set; }


    }
}
