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
    /// 考试违纪记录
    /// </summary>
    /// 
    [Table("ExamBreach")]
    public class ExamBreach
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 考试
        /// </summary>
        public int Exam { get; set; }


        /// <summary>
        /// 学生
        /// </summary>
        public string StudentNumber { get; set; }


        /// <summary>
        /// 违纪说明
        /// </summary>
        public string Breach { get; set; }

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime CreateDate { get;set;}

    }
}
