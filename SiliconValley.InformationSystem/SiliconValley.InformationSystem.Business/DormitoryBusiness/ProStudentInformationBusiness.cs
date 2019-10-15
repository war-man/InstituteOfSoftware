using SiliconValley.InformationSystem.Entity.MyEntity;
using System.Collections.Generic;
using System.Linq;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 我自己的学生业务类
    /// </summary>
    public class ProStudentInformationBusiness : BaseBusiness<StudentInformation>
    {
        /// <summary>
        /// 获取所有的在校生
        /// </summary>
        /// <returns></returns>
        public List<StudentInformation> GetStudentInSchoolData() {
          return  this.GetIQueryable().Where(a => a.IsDelete == false&&a.State==null).ToList();
        }

        /// <summary>
        /// 根据学生编号获取学生对象
        /// </summary>
        /// <param name="StudentNumber"></param>
        /// <returns></returns>
        public StudentInformation GetStudent(string StudentNumber) {
            return this.GetStudentInSchoolData().Where(a => a.StudentNumber == StudentNumber).FirstOrDefault();
        }

    }
}
