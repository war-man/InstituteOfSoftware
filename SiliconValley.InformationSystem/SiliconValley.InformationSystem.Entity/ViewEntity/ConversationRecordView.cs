using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class ConversationRecordView
    {

        public int ID { get; set; }
        public string EmployeeId { get; set; }
        public string EmpName { get; set; }
        public string Theme { get; set; }
        public string Content { get; set; }
        public string Result { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public string StudenNumber { get; set; }
        public string StudentName { get; set; }

    }
}
