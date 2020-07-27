using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.TM_Data
{
    /// <summary>
    /// 备案数据操作日志
    /// </summary>
    [Table("StudentbeanLog")]
    public class StudentbeanLog
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 员工编号
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 操作内容
        /// </summary>
        public string operationType { get; set; }
        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime insertDate { get; set; }
    }
}
