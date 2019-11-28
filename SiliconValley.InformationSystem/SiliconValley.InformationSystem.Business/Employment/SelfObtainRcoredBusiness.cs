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
    /// 自主就业业务类
    /// </summary>
   public class SelfObtainRcoredBusiness:BaseBusiness<SelfObtainRcored>
    {

        private QuarterBusiness dbquarter;
        private ProScheduleForTrainees dbproScheduleForTrainees;
        /// <summary>
        /// 获取全部可用的深数据
        /// </summary>
        /// <returns></returns>
        public List<SelfObtainRcored> GetSelfObtainRcoreds() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据学生编号获取这个自主就业的对象
        /// </summary>
        /// <param name="Studentno"></param>
        /// <returns></returns>
        public SelfObtainRcored GetSelfObtainByStudentno(string Studentno)
        {
           return this.GetSelfObtainRcoreds().Where(a => a.StudentNO == Studentno).FirstOrDefault();
        }

        /// <summary>
        ///根据这个计划id 获取这个计划中自主就业的对象
        /// </summary>
        /// <param name="QuarterID"></param>
        /// <returns></returns>
        public List<SelfObtainRcored> GetSelfObtainsByQuarterID(int QuarterID) {
            return this.GetSelfObtainRcoreds().Where(a => a.QuarterID == QuarterID).ToList();
        }


        /// <summary>
        ///根据年度获取这个年度所有的自主就业的数据
        /// </summary>
        /// <param name="Year"></param>
        /// <returns></returns>
        public List<SelfObtainRcored> GetSelfObtainRcoredsByYear(int Year) {
            dbquarter = new QuarterBusiness();
            var data = this.GetSelfObtainRcoreds();
            var list = dbquarter.GetQuartersByYear(Year);
            for (int i = data.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (data[i].QuarterID != list[j].ID)
                    {
                        if (j == list.Count - 1)
                        {
                            data.Remove(data[i]);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return data;
        }

        /// <summary>
        /// 根据班级编号返回这个班级的自主就业记录
        /// </summary>
        /// <param name="classno"></param>
        /// <returns></returns>
        public List<SelfObtainRcored> GetSelfObtainRcoredsByClassno(string classno) {
            dbproScheduleForTrainees = new ProScheduleForTrainees();
            var data = this.GetSelfObtainRcoreds();
            var list1 = dbproScheduleForTrainees.GetTraineesByClassNO(classno);
            for (int i = data.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < list1.Count; j++)
                {
                    if (data[i].StudentNO != list1[j].StudentID)
                    {
                        if (j == list1.Count - 1)
                        {
                            data.Remove(data[i]);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return data;
        }



    }
}
