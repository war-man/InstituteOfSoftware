using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class EnrollmentStudentView
    {
        public string StudentNumber { get; set; }
        public string Name { get; set; }
        public string identitydocument { get; set; }
        public string ClassName { get; set; }
        public string Headmasters { get; set; }
        public string School { get; set; }
        public string Registeredbatch { get; set; }
        public string PassNumber { get; set; }
        public string MajorID { get; set; }
        public int Alreadypassed { get; set; }
    }
}
