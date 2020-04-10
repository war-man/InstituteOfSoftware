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
    /// 满意度调查对象
    /// </summary>
    /// 
    [Table("SatisfactionSurveyObject")]
    public class SatisfactionSurveyObject
    {
        [Key]
        public int Id { get; set; }
        public string ObjectName { get; set; }
        /// <summary>
        /// 部门ID 用逗号分割
        /// </summary>
        public string Depids { get; set; }
    }
}
