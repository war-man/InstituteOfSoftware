using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
   public class CourseView
    {
        public int CurriculumID { get; set; }
        public  Grand Grand { get; set; }
        public Specialty Major { get; set; }
        public string CourseName { get; set; }
        public CourseType CourseType { get; set; }
        public int ? CourseCount { get; set; }
        public decimal ? PeriodMoney { get; set; }
        public string Rmark { get; set; }

        public bool IsDelete { get;set; }
    }
}
