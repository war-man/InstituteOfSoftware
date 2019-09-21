using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.MyEntity
{
    //学员学费标准
    [Table("Studenttuitionfeestandard")]
    public class Studenttuitionfeestandard
    {
        [Key]
        public int id { get; set; }
       public string Unitpricename { get; set; }
     public decimal UnitPrice { get; set; }
       public int Stage { get; set; }
     public bool IsDelete { get; set; }
    }
}
