using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
   public class ComputerTestQuestionsView
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string SaveURL { get; set; }
        /// <summary>
        /// 命题人
        /// </summary>
        public EmployeesInfo Proposition { get; set; }
        public QuestionLevel Level { get; set; }
        public Nullable<bool> IsUsing { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Curriculum Course { get; set; }
        /// <summary>
        /// 使用次数
        /// </summary>
        public int UsageCount { get; set; }
    }
}
