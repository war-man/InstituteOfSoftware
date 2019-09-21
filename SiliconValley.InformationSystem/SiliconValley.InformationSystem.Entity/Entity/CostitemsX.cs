using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.MyEntity
{
    [Table("CostitemsX")]
    //大费用名目表
   public class CostitemsX
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public bool IsDelete { get; set; }
       public DateTime AddDate { get; set; }
        public string Url { get; set; }


    }
}
