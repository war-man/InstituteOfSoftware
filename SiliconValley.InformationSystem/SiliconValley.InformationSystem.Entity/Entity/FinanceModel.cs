using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.MyEntity
{
    [Table("FinanceModel")]
    //财务人员表
   public class FinanceModel
    {
        [Key]
       public int id { get; set; }
       public string Financialstaff { get; set; }
       public bool IsDelete { get; set; }
       public DateTime AddDate { get; set; }
    }
}
