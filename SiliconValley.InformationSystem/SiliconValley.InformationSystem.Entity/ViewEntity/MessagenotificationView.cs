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
    /// 信息通知视图类
    /// </summary>
    [Table("MessagenotificationView")]
  public   class MessagenotificationView
    {
        /// <summary>
        /// 通知员工号
        /// </summary>
        [Key]
        public int id { get; set; }

        public string NotifierEmployeeId { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 通知人姓名
        /// </summary>
        public string NotifierName { get; set; }
        /// <summary>
        /// 发布人姓名
        /// </summary>
        public string PublisherName { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime Addtime { get; set; }
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool Readornot { get; set; }

    }
}
