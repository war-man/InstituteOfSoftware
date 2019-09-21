using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    public class SatisfactionSurveyDetailView
    {
        public EmployeesInfo Emp { get; set; }

        public DateTime investigationDate { get; set; }//调查时间

        public StudentInformation FillInPerson { get; set; }//填写人

        public int TotalScore { get; set; }//总分

        public string Proposal { get; set; }//建议

        public ClassSchedule investigationClass { get; set; } //班级

        public Curriculum Curriculum { get; set; }//调查的课程

        public List<SatisficingResultDetailView> detailitem;

        public int SurveyResultID { get; set; }

        

    }

}
