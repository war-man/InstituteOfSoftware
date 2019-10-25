using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity
{
    //报考科目 
    [Table(name: "Registerforexamination")]
  public  class Registerforexamination
    {
        [Key]
       public int id { get; set; }
        /// <summary>
        /// 缴费日期
        /// </summary>
       public DateTime Dateofapplication { get; set; }
        /// <summary>
        /// 学员
        /// </summary>
       public int EnrollID { get; set; }
        /// <summary>
        /// 科目
        /// </summary>
       public int UndergraduatecourseID { get; set; }
        /// <summary>
        /// 考期
        /// </summary>
        public string Examinationperiod { get; set; }
        /// <summary>
        /// 是否交齐
        /// </summary>
       public bool Whether { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }
    }
}
