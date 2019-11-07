using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
   public class AnswerQuestionView
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string ReferenceAnswer { get; set; }
        public Curriculum Course { get; set; }
        public EmployeesInfo Proposition { get; set; }
        public QuestionLevel Level { get; set; }
        public Nullable<bool> IsUsing { get; set; }
        public string Remark { get; set; }
        public Nullable<DateTime> PropositionDate { get; set; }
    }
}
