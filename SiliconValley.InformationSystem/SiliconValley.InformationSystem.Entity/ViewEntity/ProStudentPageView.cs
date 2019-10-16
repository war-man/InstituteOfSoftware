using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 作用于房间详细右侧的学员信息
    /// </summary>
   public class ProStudentPageView
    {
        public string StudentNumber { get; set; }
        public string StudentName { get; set; }
        public string StudentPhone { get; set; }
        public string StaffName { get; set; }
        public string StaffPhone { get; set; }
        public string ClassNO { get; set; }
    
    }
}
