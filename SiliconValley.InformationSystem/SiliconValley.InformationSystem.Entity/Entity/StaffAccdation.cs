using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table("StaffAccdation")]
    /// <summary>
    /// 员工居住信息
    /// </summary>
   public class StaffAccdation
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 入住时间
        /// </summary>
        public DateTime StayDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public Nullable<System.DateTime> EndDate { get; set; }

        public string EmployeeId { get; set; }

        /// <summary>
        /// 床位id
        /// </summary>
        public int BedId { get; set; }

        public string Remark { get; set; }
        public bool IsDel { get; set; }

        /// <summary>
        ///房间id
        /// </summary>
        public int DormId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
