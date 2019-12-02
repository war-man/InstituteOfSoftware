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
    /// 就业部调研学生访谈业务类
    /// </summary>
    public class SurveyRecordsBusiness : BaseBusiness<SurveyRecords>
    {
        private ProScheduleForTrainees dbproScheduleForTrainees;
        /// <summary>
        /// 获取全部数据 没有伪删除的数据
        /// </summary>
        /// <returns></returns>
        public List<SurveyRecords> GetSurveys() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

        /// <summary>
        /// 根据班级编号返回学生访谈记录
        /// </summary>
        /// <param name="classno"></param>
        /// <returns></returns>
        public List<SurveyRecords> GetSurveyRecordsByclassno(int  classid) {
            var list = this.GetSurveys();
            List<SurveyRecords> dd = new List<SurveyRecords>();
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
        ///获取cd类学生记录
        /// </summary>
        /// <returns></returns>
        public List<SurveyRecords> GetCDSurveyRecords() {
            return this.GetSurveys().Where(a => a.SurRating.ToUpper() == "C" || a.SurRating.ToUpper() == "D").ToList();
        }

        /// <summary>
        /// 根据班级获取CD类学生
        /// </summary>
        /// <param name="classno"></param>
        /// <returns></returns>
        public List<SurveyRecords> GetCDSurveyRecordsByclassid(int classid)
        {
            var list = this.GetCDSurveyRecords();
            List<SurveyRecords> dd = new List<SurveyRecords>();
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
    }
}
