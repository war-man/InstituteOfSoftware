using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView
{

    /// <summary>
    /// 安排阅卷模型
    /// </summary>
   public class MarkingArrangeView
    {
        public int ID { get; set; }

        /// <summary>
        /// 阅卷老师
        /// </summary>
        public EmployeesInfo MarkingTeacher { get; set; }
        /// <summary>
        /// 考试ID
        /// </summary>

        public Examination ExamID { get; set; }
        /// <summary>
        /// 考场
        /// </summary>

        public ExaminationRoom ExamRoom { get; set; }

        /// <summary>
        /// 所在教室
        /// </summary>
        public Classroom classroom { get; set; }

        /// <summary>
        /// 是否已完成阅卷
        /// </summary>

        public bool IsFinsh { get; set; }
    }
}
