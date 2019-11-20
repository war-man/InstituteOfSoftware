using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table(name: "MarkingArrange")]
    public class MarkingArrange
    {
        [Key]
        public int ID { get; set; }

        /// <summary>
        /// 阅卷老师
        /// </summary>
        public string MarkingTeacher { get; set; }

        public int ExamID { get; set; }

        public int ExamRoom { get; set; }

        public bool IsFinsh { get; set; }
    }
}
