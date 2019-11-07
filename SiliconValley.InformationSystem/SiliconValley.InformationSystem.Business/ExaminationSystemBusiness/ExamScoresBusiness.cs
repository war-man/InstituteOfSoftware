using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.ExaminationSystemBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    //考试成绩业务类
    public class ExamScoresBusiness : BaseBusiness<TestScore>
    {

        public List<TestScore> AllExamScores()
        {
            return this.GetList().ToList();
        }


        /// <summary>
        /// 获取考试成绩
        /// </summary>
        /// <returns></returns>
        public List<TestScore> ExamScores(int examid)
        {
           return this.AllExamScores().Where(d => d.Examination == examid).ToList();
        }

    }
}
