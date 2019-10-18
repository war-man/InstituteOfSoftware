using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{

    [Table("DormitoryLeader")]
   public class DormitoryLeader
    {
        /// <summary>
        /// 主键id
        /// </summary>
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 房间id
        /// </summary>
        public int DormInfoID { get; set; }
        

        /// <summary>
        /// 学生编号
        /// </summary>
        public string StudentNumber { get; set; }

        public bool IsDelete { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
