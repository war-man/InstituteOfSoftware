using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table("HeadMasterGrand")]
    public class HeadMasterGrand
    {
        [Key]
        public int ID { get; set; }

        public int GrandID { get; set; }
        public int HeadMasterID { get; set; }
        public bool IsDel { get; set; }
    }
}
