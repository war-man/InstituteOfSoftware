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
    /// 短信模板类型
    /// </summary>
    [Table(name: "ShortmessageType")]
  public  class ShortmessageType
    {
        [Key]
      public int id { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
      public string TypeName { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
      public bool IsDelete { get; set; }
    }
}
