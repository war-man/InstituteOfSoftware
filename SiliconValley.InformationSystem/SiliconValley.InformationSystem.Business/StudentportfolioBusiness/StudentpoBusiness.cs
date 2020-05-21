using SiliconValley.InformationSystem.Business.ClassesBusiness;
using SiliconValley.InformationSystem.Business.ClassSchedule_Business;
using SiliconValley.InformationSystem.Business.StudentBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.StudentportfolioBusiness
{
  public  class StudentpoBusiness
    {
     
        /// 学员信息
        StudentInformationBusiness studentInformation = new StudentInformationBusiness();
        //学员班级
        ClassScheduleBusiness classScheduleBusiness = new ClassScheduleBusiness();
        //学员班级表
        ScheduleForTraineesBusiness scheduleForTraineesBusiness = new ScheduleForTraineesBusiness();
        /// <summary>
        /// 获取学员基础信息
        /// </summary>
        /// <param name="Studentid"></param>
        /// <returns></returns>
        public object Studentfine(string Studentid)
        {
            var student = studentInformation.GetEntity(Studentid);
            var stu = new
            {
                scheduleForTraineesBusiness.SutdentCLassName(Studentid).ClassID,
                student.Picture,
                student.Name,
                student.Nation,
                student.Telephone,
                student.StudentNumber,
                student.identitydocument,
                student.Hobby,
                student.Guardian,
                student.Familyphone,
                student.Familyaddress,
                student.BirthDate,
                student.Education,
                student.Sex,
                student.Identitybackimg,
                student.Identityjustimg
            };

            return stu;
        }
    }
}
