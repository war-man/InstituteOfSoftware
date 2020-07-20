using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table(name: "Undergraduatemajor")]
    //本科专业表
   public class Undergraduatemajor
    {
        [Key]
      public int id { get; set; }
       public string Professionalcode { get; set; }
        /// <summary>
        /// 专业名称
        /// </summary>
      public string ProfessionalName { get; set; }
        public bool IsDelete { get; set; }
    }
}
