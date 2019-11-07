using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    /// <summary>
    /// 考场视图模型
    /// </summary>
    public class ExaminationRoomView
    {
        public int ID { get; set; }
        public Classroom Classroom { get; set; }
        public EmployeesInfo Invigilator1 { get; set; }
        public EmployeesInfo Invigilator2 { get; set; }
        public string Remark { get; set; }
        public Examination Examination { get; set; }
    }
}
