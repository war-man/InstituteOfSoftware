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
    /// 短信发送内容模板
    /// </summary>
    [Table(name: "Shortmessage")]
   public class Shortmessage
    {
        [Key]
       public int id { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
       public int TypeID { get; set; } 
        /// <summary>
        /// 内容
        /// </summary>
       public string content { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
       public DateTime AddDate { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
       public bool IsDelete { get; set; }
    }
}
