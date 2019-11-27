using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class ClassTableView
    {

        public int classid { get; set; }
        public string ClassNumber { get; set; }
        public Nullable<bool> ClassStatus { get; set; }
        public string ClassRemarks { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string MajorName { get; set; }
        public string GradeName { get; set; }

        public string Headmaster { get; set; }//班主任

        public string qqGroup { get; set; } //qq群号

        public int ClassSize { get; set; }//班级人数

    }
}
