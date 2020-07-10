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
    /// 迟到学生详细表
    /// </summary>
    [Table("Laterecord_StudentAttendance")]
    public class Laterecord_StudentAttendance
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 迟到记录编号
        /// </summary>
        public int Laterecord_Id { get; set; }
        /// <summary>
        /// 学生姓名
        /// </summary>
        public string StudentName { get; set; }
    }
}
