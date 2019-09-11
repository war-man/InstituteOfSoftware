using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class StuHomeWorkView
    {
        public int ID { get; set; }
        public string ClassroomHomeWork { get; set; }
        public string AfterClassHomeWork { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> CheckDate { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public Nullable<int> ChekTeacher { get; set; }
        public string StudentNumber { get; set; }

        public string StudentName { get; set; }
        public DateTime ReleaseDate { get; set; }//作业布置的时间
    }
}
