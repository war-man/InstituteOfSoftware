using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table("TeacherHourCost")]
    /// <summary>
    /// 阶段课时费
    /// </summary>
    public class TeacherHourCost
    {
        [Key]
        public int ID { get; set; }


        /// <summary>
        /// 阶段
        /// </summary>
        public int Grand { get; set; }

        /// <summary>
        /// 费用
        /// </summary>
        public float Cost { get; set; }

        public bool IsUsing { get; set; }
        
    }
}
