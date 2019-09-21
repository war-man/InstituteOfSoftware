using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.MyEntity
{
    //小名目
    [Table("Costitems")]
  public  class Costitems
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
       
        public decimal Amountofmoney { get; set; }
        public int? Grand_id { get; set; }
        public bool IsDelete { get; set; }

        public int Rategory { get; set; }
    }
}
