using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 学生班级业务类
    /// </summary>
    public class ProScheduleForTrainees : BaseBusiness<ScheduleForTrainees>
    {

        private ProClassSchedule dbproClassSchedule;
        private ProStudentInformationBusiness dbproStudentInformation;


        public List<ScheduleForTrainees> GetScheduleForTraineesIng() {

            return this.GetIQueryable().Where(a => a.CurrentClass == true).ToList();
        }
        /// <summary>
        ///获取班级学生记录没毕业的
        /// </summary>
        /// <returns></returns>
        public List<ScheduleForTrainees> GetScheduleForTrainees()
        {
            return this.GetScheduleForTraineesIng().Where(a => a.IsGraduating == false).ToList();

        }

        /// <summary>
        ///获取班级学生记录毕业的
        /// </summary>
        /// <returns></returns>
        public List<ScheduleForTrainees> GetScheduleForTraineesed()
        {
            return this.GetIQueryable().Where(a => a.IsGraduating == true).ToList();

        }

        /// <summary>
        /// 根据学生编号获取学生班级记录
        /// </summary>
        /// <param name="StudentNumber"></param>
        /// <returns></returns>
        public ScheduleForTrainees GetTraineesByStudentNumber(string StudentNumber)
        {
            dbproStudentInformation = new ProStudentInformationBusiness();
            var query= dbproStudentInformation.GetEntity(StudentNumber);

            if (query.State==1)
            {
                return this.GetScheduleForTraineesed().Where(a => a.StudentID == StudentNumber).FirstOrDefault();
            }
            else
            {
                return this.GetScheduleForTrainees().Where(a => a.StudentID == StudentNumber).FirstOrDefault();
            }
           

        } 
      

        /// <summary>
        /// 根据班级编号获取学生班级记录
        /// </summary>
        /// <param name="ClassNO"></param>
        /// <returns></returns>
        public List<ScheduleForTrainees> GetTraineesByClassid(int classid)
        {
            dbproClassSchedule = new ProClassSchedule();

            ///判断是否是毕业班级
            if (dbproClassSchedule.isgraduationclass(classid))
            {
                return this.GetScheduleForTraineesed().Where(a => a.ID_ClassName == classid).ToList();
            }
            else
            {
                return this.GetScheduleForTrainees().Where(a => a.ID_ClassName == classid).ToList();
            }
        }


        /// <summary>
        /// 根据班级编号返回学生列表集合
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        public List<StudentInformation> GetStudentsByClassid(int classid)
        {
            dbproStudentInformation = new ProStudentInformationBusiness();
            var querylist = this.GetTraineesByClassid(classid);
            List<StudentInformation> result = new List<StudentInformation>();
            foreach (var item in querylist)
            {
                result.Add(dbproStudentInformation.GetEntity(item.StudentID));
            }
            return result;
        }
    }
}
