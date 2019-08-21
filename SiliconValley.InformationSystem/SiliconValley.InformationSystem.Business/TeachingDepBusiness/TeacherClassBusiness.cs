using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
   public class TeacherClassBusiness:BaseBusiness<ClassTeacher>
    {

        public List<ClassTeacher> GetClassTeachers()
        {
            return this.GetList().Where(d => d.IsDel == false).ToList() ;
        }



        /// <summary>
        /// 获取我的班级
        /// </summary>
        /// <param name="teacherid">老师ID</param>
        /// <returns>班级集合</returns>

        public List<ClassSchedule> GetCrrentMyClass(int teacherid)
        {

           var templist =  this.GetClassTeachers().Where(d=>d.TeacherID==teacherid).ToList();

            BaseBusiness<ClassSchedule> classdb = new BaseBusiness<ClassSchedule>();

            return null;

        }

    }
}
