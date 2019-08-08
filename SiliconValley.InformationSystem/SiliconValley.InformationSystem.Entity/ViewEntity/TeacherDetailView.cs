using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
  public  class TeacherDetailView
    {

        public int TeacherID { get; set; }//教员ID

        public string Name { get; set; }//姓名

        public string Sex { get; set; }//性别

        public string EmpNo { get; set; } //员工编号

        public string Phone { get; set; }//电话

        public string Birthday { get; set; } //生日

        public List<Specialty> Major { get; set; } //专业

        public List<Grand> Grands { get; set; } //阶段

        public string ProjectExperience { get; set; } //项目经验

        public string AttendClassStyle { get; set; } //教学风格

        public string TeachingExperience { get; set; } //教学经验

        public string WorkExperience { get; set; } //工作经验


    }
}
