using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
  public  class TeacherDetailView
    {

        public int TeacherID { get; set; }//教员ID

        public EmployeesInfo emp { get; set; }

        public Department Department { get; set; }
       

        public Position Position { get; set; }
       

        public List<Specialty> Major { get; set; } //专业

        public List<Grand> Grands { get; set; } //阶段

        public string ProjectExperience { get; set; } //项目经验

        public string AttendClassStyle { get; set; } //教学风格

        public string TeachingExperience { get; set; } //教学经验

        public string WorkExperience { get; set; } //工作经验


    }



}
