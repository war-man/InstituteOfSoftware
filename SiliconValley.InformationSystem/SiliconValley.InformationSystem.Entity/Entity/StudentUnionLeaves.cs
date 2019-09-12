using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.MyEntity
{
    [Table("StudentUnionLeaves")]
    public class StudentUnionLeaves
    {
        [Key]
        public int id { get; set; }
        public int Union_id { get; set; }
        public string Reason { get; set; }
        public DateTime Datetimes { get; set; }
    }
}
