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
    /// 专员带班异动记录
    /// </summary>
    [Table("EmpClassTransaction")]
    public class EmpClassTransaction
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 前任专员
        /// </summary>
        public int FormerEmpID { get; set; }

        /// <summary>
        /// 现任专员
        /// </summary>
        public int IncumbentEmpID { get; set; }

        /// <summary>
        /// 异动时间
        /// </summary>
        public DateTime TryData { get; set; }
        /// <summary>
        /// 异动原因
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }
        /// <summary>
        /// 异动类型，0 开除 1删除
        /// </summary>
        public int Type { get; set; }
    }
}
