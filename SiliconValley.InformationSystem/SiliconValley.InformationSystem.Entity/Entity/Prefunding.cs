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
    /// 预资表
    /// </summary>
    [Table("Prefunding")]
    public class Prefunding
    {
        /// <summary>
        /// 预约单id
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmpNumber { get; set; }

        /// <summary>
        /// 预资时间
        /// </summary>
        public DateTime PreDate { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDel { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 预资金额
        /// </summary>
        public double PerMoney { get; set; }
    }
}
