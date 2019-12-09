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
    /// CD类访谈记录业务类
    /// </summary>
   public class CDInterviewBusiness:BaseBusiness<CDInterview>
    {
        private ProScheduleForTrainees dbproScheduleForTrainees;
        public List<CDInterview> GetCDInterviews() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据班级号获cd类访谈记录
        /// </summary>
        /// <param name="classno"></param>
        /// <returns></returns>
        public List<CDInterview> GetCDInterviewsByClassid(int classid)
        {
            var list = this.GetCDInterviews();
            List<CDInterview> dd = new List<CDInterview>();
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            var list1 = dbproScheduleForTrainees.GetTraineesByClassid(classid);
            foreach (var item in list)
            {
                foreach (var item1 in list1)
                {
                    if (item.StudentNO == item1.StudentID)
                    {
                        dd.Add(item);
                    }
                }
            }
            return dd;
        }

        /// <summary>
        /// 根据学生编号返回这个CD类
        /// </summary>
        /// <param name="Studentnumber"></param>
        /// <returns></returns>
        public List<CDInterview> GetCDsByStudentnumber(string Studentnumber) {
          return   this.GetCDInterviews().Where(a => a.StudentNO == Studentnumber).ToList();
        }

        /// <summary>
        /// 删除传入过来的集合数据
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool Dellist(List<CDInterview> data) {
            bool result = true;
            try
            {
                foreach (var item in data)
                {
                    item.IsDel = true;
                    this.Update(item);
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public void back(List<CDInterview> data) {
            foreach (var item in data)
            {
                item.IsDel = false;
                this.Update(item);
            }
        }
    }
}
