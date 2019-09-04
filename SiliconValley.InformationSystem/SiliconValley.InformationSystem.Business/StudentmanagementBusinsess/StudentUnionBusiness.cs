using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.StudentmanagementBusinsess
{
    //学生会
  public class StudentUnionBusiness:BaseBusiness<StudentUnionMembers>
    {
        //学员信息
        BaseBusiness<StudentInformation> Student = new BaseBusiness<StudentInformation>();
        //班级学员
        BaseBusiness<ScheduleForTrainees> ClassSche = new BaseBusiness<ScheduleForTrainees>();
        //学生会部门
        BaseBusiness<StudentUnionDepartment> UnionDepart = new BaseBusiness<StudentUnionDepartment>();
        //学生会职位
        BaseBusiness<StudentUnionPosition> SUnionPosition = new BaseBusiness<StudentUnionPosition>();
        /// <summary>
        /// 获取学生会所有成员
        /// </summary>
        /// <returns></returns>
        public object UnionMembersList()
        {
          return  this.GetList().Where(a => a.Dateofregistration == false).Select(
                aa => new
                {
                    Stuentnumber = aa.Studentnumber,
                    StidentName = Student.GetList().Where(a => a.IsDelete == false && a.State == null && a.StudentNumber == aa.Studentnumber).First().Name,
                    StudentSex = Student.GetList().Where(a => a.IsDelete == false && a.State == null && a.StudentNumber == aa.Studentnumber).First().Sex,
                    ClassName = ClassSche.GetList().Where(a => a.CurrentClass == true && a.StudentID == aa.Studentnumber).First().ClassID,
                    Department= UnionDepart.GetList().Where(a=>a.Dateofregistration==false&&a.ID==aa.department).First().Departmentname,
                    Position= SUnionPosition.GetList().Where(a=>a.Dateofregistration==false&&a.ID==aa.position).First().Jobtitle
                });
        }

        public bool UnionMembersEntity(StudentUnionMembers studentUnionMembers)
        {
            return false;
        }
    }
}
