using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table("GrandCourse")]
    public class GrandCourse
    {
        [Key]
        public int ID { get; set; }

        public int courseId { get; set; }
        public int GrandId { get; set; }
        public bool isDel { get; set; }
    }
}
