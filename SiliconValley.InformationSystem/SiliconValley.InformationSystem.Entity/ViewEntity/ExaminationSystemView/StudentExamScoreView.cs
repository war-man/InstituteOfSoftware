using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity;
    /// <summary>
    /// 学员考试成绩
    /// </summary>
   public class StudentExamScoreView
    {
        public string StudentName { get; set; }

        public string StudentNumber { get; set; }

        public string StudentClass { get; set; }

       

        /// <summary>
        /// 阅卷老师
        /// </summary>
        public string MarkingTeacherName { get; set; }


        /// <summary>
        /// 成绩
        /// </summary>
        public TestScore Score { get; set; }


        /// <summary>
        /// 考试名称
        /// </summary>
        public string ExamTitle { get; set; }
    }
}
