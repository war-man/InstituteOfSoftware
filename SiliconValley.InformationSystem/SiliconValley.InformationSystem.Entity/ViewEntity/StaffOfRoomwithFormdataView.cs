using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class StaffOfRoomwithFormdataView
    {
        public string EmployeeId { get; set; }

        public string EmpName { get; set; }

        public string DeptName { get; set; }

        public string PositionName { get; set; }
        /// <summary>
        /// 入住时间
        /// </summary>
        public DateTime StayDate { get; set; }

        public string Phone { get; set; }
    }
}
