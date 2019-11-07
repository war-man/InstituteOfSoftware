using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    //本科报考学校表
    [Table(name: "Undergraduateschool")]
  public  class Undergraduateschool
    {
        [Key]
     public int id { get; set; }
     public string SchoolName { get; set; }
    public bool IsDelete { get; set; }
    }
}
