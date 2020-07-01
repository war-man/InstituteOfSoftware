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
    /// 系统消息通知
    /// </summary>
    [Table("Messagenotification")]
  public  class Messagenotification
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 到期日期
        /// </summary>
        public DateTime? Duedate { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 发布人
        /// </summary>
        public string PublisherEmployeeId { get; set; }
      
        
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime Addtime { get; set; }

    }
}
