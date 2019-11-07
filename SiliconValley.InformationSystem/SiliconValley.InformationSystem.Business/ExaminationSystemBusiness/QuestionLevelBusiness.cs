using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.ExaminationSystemBusiness
{

    using SiliconValley.InformationSystem.Entity.MyEntity;
    /// <summary>
    /// 题目级别业务类
    /// </summary>
   public class QuestionLevelBusiness:BaseBusiness<QuestionLevel>
    {

        public List<QuestionLevel> AllQuestionLevel()
        {
            return this.GetList().ToList();
        }


    }
}
