using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView
{
   public class MarkingArrangeView
    {
        public int ID { get; set; }

        /// <summary>
        /// 阅卷老师
        /// </summary>
        public EmployeesInfo MarkingTeacher { get; set; }

        public Examination ExamID { get; set; }

        public ExaminationRoom ExamRoom { get; set; }

        public bool IsFinsh { get; set; }
    }
}
