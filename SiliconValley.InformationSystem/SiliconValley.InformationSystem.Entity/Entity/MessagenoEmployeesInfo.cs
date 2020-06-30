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
    /// 消息通知关系表
    /// </summary>
    [Table("MessagenoEmployeesInfo")]
   public class MessagenoEmployeesInfo
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 通知内容id
        /// </summary>
        public int MessagenoID { get; set; }
        /// <summary>
        /// 读取人
        /// </summary>
        public string NotifierEmployeeId { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool Readornot { get; set; }
        /// <summary>
        /// 读取时间
        /// </summary>
        public DateTime? AddTimn { get; set; }

       
    }
}
