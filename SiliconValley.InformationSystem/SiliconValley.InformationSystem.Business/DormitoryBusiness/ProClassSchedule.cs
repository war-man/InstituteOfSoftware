using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
    /// <summary>
    /// 班级业务类
    /// </summary>
   public class ProClassSchedule:BaseBusiness<ClassSchedule>
    {
        /// <summary>
        /// 获取没有毕业的还在用的班级
        /// </summary>
        /// <returns></returns>
        public List<ClassSchedule> GetClassSchedules() {
            return this.GetIQueryable().Where(a => a.IsDelete == false && a.ClassStatus == false).ToList();
            
        }

        /// <summary>
        /// 根据班级编号获取正在使用的班级
        /// </summary>
        /// <param name="ClassNumber"></param>
        /// <returns></returns>
        public ClassSchedule GetNotgraduatedClassByClassNumber(string ClassNumber) {
            return this.GetClassSchedules().Where(a => a.ClassNumber == ClassNumber).FirstOrDefault();
        }
    }
}
