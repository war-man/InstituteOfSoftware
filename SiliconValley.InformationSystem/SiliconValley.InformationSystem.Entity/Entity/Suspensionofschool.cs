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
    /// 休学表
    /// </summary>
    [Table(name: "Suspensionofschool")]
   public class Suspensionofschool
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
        /// 开始休学日期
        /// </summary>
       public DateTime Startingperiod { get; set; }
        /// <summary>
        /// 结束休学日期
        /// </summary>
       public DateTime Deadline { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
       public string Reason { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
       public bool IsDelete { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
       public string Remarks { get; set; }
        /// <summary>
        /// 申请日期
        /// </summary>
       public DateTime Dateofapplication { get; set; }
    }
}
