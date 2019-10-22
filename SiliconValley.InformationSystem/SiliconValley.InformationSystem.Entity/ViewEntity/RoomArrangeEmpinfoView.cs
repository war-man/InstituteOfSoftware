using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于房间分配
    /// </summary>
    public class RoomArrangeEmpinfoView
    {
        public string EmployeeId { get; set; }
        public string EmpName { get; set; }
        public string Phone { get; set; }
        public string Sex { get; set; }
        public string DeptName { get; set; }
        public int DormID { get; set; }
        public string DormName { get; set; }

        public string datatype { get; set; }
    }
}
