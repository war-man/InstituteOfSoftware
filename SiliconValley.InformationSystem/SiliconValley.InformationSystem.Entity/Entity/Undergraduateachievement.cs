using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    //本科成绩表                                                                  
    [Table(name: "Undergraduateachievement")]
   public class Undergraduateachievement
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 科目
        /// </summary>
        public int Subjectid { get; set; }
        /// <summary>
        /// 成绩
        /// </summary>
        public int Fraction { get; set; }
        /// <summary>
        /// 本科学员信息
        /// </summary>
        public int EnrollID { get; set; }
        /// <summary>
        /// 考期
        /// </summary>
        public string Examinationperiod { get; set; }
    }
}
