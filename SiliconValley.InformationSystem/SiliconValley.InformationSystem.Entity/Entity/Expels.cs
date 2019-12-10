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
    /// 开除表
    /// </summary>
    [Table(name: "Expels")]
   public  class Expels
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string Studentnumber { get; set; }
        /// <summary>
        /// 开除原因
        /// </summary>
        public string Reason { get; set;}
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
        /// <summary>
        /// 开除日期
        /// </summary>
        public DateTime Applicationtime { get; set; }
    }
}
