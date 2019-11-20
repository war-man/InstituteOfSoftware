using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table(name: "ExamTypeName")]
    public class ExamTypeName
    {
        [Key]
        public int ID { get; set; }

        public string TypeName { get; set; }
    }
}
