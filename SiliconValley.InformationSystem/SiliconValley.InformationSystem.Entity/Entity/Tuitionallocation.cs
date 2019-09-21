using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.MyEntity
{
    [Table("Tuitionallocation")]
    public  class Tuitionallocation
    {
        [Key]
        public int id { get; set; }
        public int Latestdays { get; set; }
        public decimal Minimumcost { get; set; }
        public int Stage { get; set; }
        public DateTime AddTime { get; set; }
        public string Explain { get; set; }
        public bool IsDelete { get; set; }
    }
}
