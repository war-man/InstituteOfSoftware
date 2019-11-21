using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    /// <summary>
    /// 班级状态表
    /// </summary>
    [Table(name: "Classstatus")]
    public class Classstatus
    {
        public int id { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
