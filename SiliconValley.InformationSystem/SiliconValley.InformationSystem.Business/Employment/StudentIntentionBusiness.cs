using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 就业学生的意向填写单
    /// </summary>
   public class StudentIntentionBusiness:BaseBusiness<StudnetIntention>
    {
        private ProScheduleForTrainees dbproScheduleForTrainees;
        /// <summary>
        /// 获取现在正在使用的就业意向的全部由数据
        /// </summary>
        /// <returns></returns>
        public List<StudnetIntention> GetStudentIntentions() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据学生编号返回这个意向表
        /// </summary>
        /// <param name="StudentNO"></param>
        /// <returns></returns>
        public StudnetIntention GetStudnetIntentionByStudentNO(string StudentNO)
        {
          return  this.GetStudentIntentions().Where(a => a.StudentNO == StudentNO).FirstOrDefault();
        }

        /// <summary>
        /// 根据班级编号返回该班级学生的就业意向数据
        /// </summary>
        /// <param name="classno"></param>
        /// <returns></returns>
        public List<StudnetIntention> GetStudnetIntentionsByclassno(string classno) {
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            List<StudnetIntention> result = new List<StudnetIntention>();
            var los=  dbproScheduleForTrainees.GetTraineesByClassNO(classno);
            var ddd = this.GetStudentIntentions();
            foreach (var item in ddd)
            {
                foreach (var item1 in los)
                {
                    if (item.StudentNO==item1.StudentID)
                    {
                        result.Add(item);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据计划id 以及学生id 获取这个对象
        /// </summary>
        /// <param name="quarterid"></param>
        /// <param name="studentno"></param>
        /// <returns></returns>
        public StudnetIntention GetIntention(int quarterid,string studentno)
        {
           return this.GetStudentIntentions().Where(a => a.QuarterID == quarterid && a.StudentNO == studentno).FirstOrDefault();
        }
    }
}
