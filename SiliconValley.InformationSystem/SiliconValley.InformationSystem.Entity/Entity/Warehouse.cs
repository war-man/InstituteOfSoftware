using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    /// <summary>
    /// 仓库表
    /// </summary>
    [Table(name: "Warehouse")]
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Reamk { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public System.DateTime Adddate { get; set; }
        /// <summary>
        /// 是否禁用
        /// </summary>
        public Nullable<bool> IsDelete { get; set; }
        
    }
}
