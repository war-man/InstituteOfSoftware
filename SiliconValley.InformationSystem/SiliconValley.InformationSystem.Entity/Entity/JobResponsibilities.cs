using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("JobResponsibilities")]
    public  class JobResponsibilities
    {
        [Key]
        public int ID { get; set; }
        public int EntSpeeID { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<System.DateTime> JobDate { get; set; }
    }
}
