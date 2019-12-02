using SiliconValley.InformationSystem.Business.DormitoryBusiness;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
   public class SimulatInterviewBusiness:BaseBusiness<SimulatInterview>
    {
        private ProScheduleForTrainees dbproScheduleForTrainees;
        /// <summary>
        /// 获取全部可用的数据信息
        /// </summary>
        /// <returns></returns>
        public List<SimulatInterview> GetSimulatInterviews() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据班级编号返回这个班级的模拟面试的记录
        /// </summary>
        /// <param name="classno"></param>
        /// <returns></returns>
        public List<SimulatInterview> GetSimulatInterviewsByclassid(int classid) {
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            List<SimulatInterview> list = new List<SimulatInterview>();
            var aa= dbproScheduleForTrainees.GetTraineesByClassid(classid);
            var bb = this.GetSimulatInterviews();
            foreach (var item in bb)
            {
                foreach (var item1 in aa)
                {
                    if (item.StudentNo==item1.StudentID)
                    {
                        list.Add(item);
                    }
                }
            }
            return list;
           
        }
    }
}
