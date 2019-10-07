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
    /// 栋表
    /// </summary>
    [Table(name: "Tung")]
    public class Tung
    {

        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 栋名称
        /// </summary>
        public string TungName { get; set; }
        /// <summary>
        /// 栋地址
        /// </summary>
        public string TungAddress{ get; set; }
        /// <summary>
        /// false可用，true 不可用
        /// </summary>
        public bool IsDel { get; set; }
        public string Remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
