using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    //课程类别大类型
    [Table(name: "CoursecategoryY")]
   public class CoursecategoryY
    {
        [Key]
       public int id { get; set; }
       public int MajorID { get; set; }
       public string Coursetitle { get; set; }
    }
}
