using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    public class MrDEmployStaffView
    {
        public int EmploymentStaffID { get; set; }
        public string EmpName { get; set; }
        public string EmployeeId { get; set; }
        public string Sex { get; set; }

        public string Phone { get; set; }
        public string AreaName { get; set; }
        /// <summary>
        /// 带班
        /// </summary>
        public List<string> TakeClasses { get; set; }
    }
}
