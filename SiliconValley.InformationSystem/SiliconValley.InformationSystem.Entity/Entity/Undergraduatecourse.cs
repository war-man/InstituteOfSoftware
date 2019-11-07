using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    //本科课程
    [Table(name: "Undergraduatecourse")]
   public class Undergraduatecourse
    {
        [Key]
        public int id { get; set; }
        public string Coursetitle { get; set; }
        public int Credit { get; set; }
        public Decimal Cost { get; set; }
        public int CoursecategoryXid { get; set; }
        public string Remarks { get; set; }
        public string Coursecode { get; set; }
        public bool IsDelete { get; set; }
    }
}
