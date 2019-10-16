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

        /// <summary>
        ///获取班级学生记录
        /// </summary>
        /// <returns></returns>
        public List<ScheduleForTrainees> GetScheduleForTrainees()
        {
            return this.GetIQueryable().Where(a => a.CurrentClass == true).ToList();
        }

        /// <summary>
        /// 根据学生编号获取学生班级记录
        /// </summary>
        /// <param name="StudentNumber"></param>
        /// <returns></returns>
        public ScheduleForTrainees GetTraineesByStudentNumber(string StudentNumber) {
            return this.GetScheduleForTrainees().Where(a => a.StudentID == StudentNumber).FirstOrDefault();
        }

        /// <summary>
        /// 根据班级编号获取学生班级记录
        /// </summary>
        /// <param name="ClassNO"></param>
        /// <returns></returns>
        public List<ScheduleForTrainees> GetTraineesByClassNO(string ClassNO) {
            return this.GetScheduleForTrainees().Where(a => a.ClassID == ClassNO).ToList();
        }

    }
}
