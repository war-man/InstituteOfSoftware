using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    public class ChoiceQuestionTableView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public Nullable<bool> IsRadio { get; set; }
        public QuestionLevel Level { get; set; }
        public string Answer { get; set; }
        public EmployeesInfo Proposition { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<bool> IsUsing { get; set; }
        public string Remark { get; set; }
        public Curriculum Course { get; set; }
    }
}
