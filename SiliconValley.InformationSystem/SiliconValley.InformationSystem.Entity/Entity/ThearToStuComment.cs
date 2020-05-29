using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.Entity
{
    [Table(name: "ThearToStuComment")]
    public class ThearToStuComment
    {
        [Key]
        public int Id { get; set; }

        public string CommnetEr { get; set; }

        public string CommnetObj { get; set; }

        public string Commnet { get; set; }

        public DateTime CommnetDate { get; set; }
    }
}
