using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    [Table("StudentpaymentView")]
 public  class StudentpaymentView
    {
        [Key]
        public string StudentNumber { get; set; }

        public string Name { get; set; }

        public bool Sex { get; set; }

        public DateTime? BirthDate { get; set; }

        public string identitydocument { get; set; }

        public string ClassName { get; set; }

        public string Headmasters { get; set; }
    }
}
