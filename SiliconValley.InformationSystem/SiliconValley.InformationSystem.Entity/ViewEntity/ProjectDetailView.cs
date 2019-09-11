using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
   public class ProjectDetailView
    {
        public int ProjectID { get; set; }
      
        public Teacher Tutor { get; set; }
        public EmployeesInfo TutorEmp { get; set; }
        public string ProjectName { get; set; }
        public Nullable<System.DateTime> BeginDate { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public string Remark { get; set; }
        public ProjectType ProjectType { get; set; }
        public StudentInformation GroupLeader { get; set; }

        public bool IsStop { get; set; }  //项目是已经终止开发

        public Nullable<System.DateTime> EndDate { get; set; } //项目结束日期

        public string ProjectIntroduce { get; set; } //项目介绍

        public List<StudentInformation> TeamImte { get; set; }
        public string ShowImages { get; set; }


    }
}
