using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;
using SiliconValley.InformationSystem.Business.StudentBusiness;
namespace SiliconValley.InformationSystem.Business.ClassesBusiness
{

  public  class ScheduleForTraineesBusiness:BaseBusiness<ScheduleForTrainees>
    {


        //通过班级名称获取学员
        public List<StudentInformation> ClassStudent(string ClassID)
        {
            //CurrentClass是否为正常状态 查询使用以参数为班级的学员学号
            var stuid = this.GetList().Where(a => a.ClassID == ClassID && a.CurrentClass == true).FirstOrDefault().StudentID;
            //查询班级所有学生
            StudentInformationBusiness student = new StudentInformationBusiness();
           return student.GetList().Where(a => a.StudentNumber == stuid).ToList();
        }
    }
}
