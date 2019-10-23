using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
  public  class StaffaccdationView
    {

        public int ID { get; set; }
        /// <summary>
        /// 床位id
        /// </summary>
        public int BedId { get; set; }

        public string EmployeeId { get; set; }

        public string EmpName { get; set; }
    }
}
