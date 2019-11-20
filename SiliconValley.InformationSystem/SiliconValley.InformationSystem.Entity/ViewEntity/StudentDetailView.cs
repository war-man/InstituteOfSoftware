using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class StudentDetailView
    {
        public string StudentNumber { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string Telephone { get; set; }
        public string qq { get; set; }
        public string WeChat { get; set; }
        public string Picture { get; set; }
        public Nullable<int> State { get; set; }
        public string Familyaddress { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string Nation { get; set; }
        public string Education { get; set; }
        public string GrandName { get; set; }

        public string MajorName { get; set; }
        public string ClassName { get; set; }
        public string PositionName { get; set; }
        
        public string IdCard { get; set; }

    }
}
