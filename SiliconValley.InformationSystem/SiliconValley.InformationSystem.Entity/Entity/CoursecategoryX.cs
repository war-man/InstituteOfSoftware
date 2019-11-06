using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    //课程类别小的 
    [Table(name: "CoursecategoryX")]
  public  class CoursecategoryX
    {
        [Key]
      public int id { get; set; }
      public string Coursetitle { get; set; }
      public int CoursecategoryYID { get; set; }
    }
}
