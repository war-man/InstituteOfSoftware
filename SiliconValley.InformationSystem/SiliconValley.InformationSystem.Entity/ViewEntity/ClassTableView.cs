using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class ClassTableView
    {


        public string ClassNumber { get; set; }
        public Nullable<bool> ClassStatus { get; set; }
        public string ClassRemarks { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string MajorName { get; set; }
        public string GradeName { get; set; }

    }
}
