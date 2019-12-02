using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
   public class SatisficingConfigView
    {
        public int ID { get; set; }
        public  Curriculum CurriculumID { get; set; }
        public EmployeesInfo EmployeeId { get; set; }
        public ClassSchedule ClassNumber { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<bool> IsPastDue { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDel { get; set; }


        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime CutoffDate { get; set; }
    }
}
