using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.ExaminationSystemBusiness
{


    using SiliconValley.InformationSystem.Entity.MyEntity;

    /// <summary>
    /// 试卷业务类
    /// </summary>
   public class ExaminationPaperBusiness:BaseBusiness<ExaminationPaper>
    {
        /// <summary>
        /// 选择题业务类实例
        /// </summary>
        private readonly BaseBusiness<ChoiceQuestionPaper> db_ChoiceQuestionPaper;

        /// <summary>
        /// 问答题业务类实例
        /// </summary>
        private readonly BaseBusiness<AnswerQuestionPaper> db_AnswerQuestionPaper;

        /// <summary>
        /// 机试题业务类实例
        /// </summary>
        private readonly BaseBusiness<MachineTestQuestionPaper> db_MachineTestQuestionPaper;

        /// <summary>
        /// 试卷类型(升学试卷，课程考试试卷)
        /// </summary>
        private readonly BaseBusiness<ExamType> db_examtypes;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ExaminationPaperBusiness()
        {
            //对业务类进行实例化

            db_AnswerQuestionPaper = new BaseBusiness<AnswerQuestionPaper>();
            db_ChoiceQuestionPaper = new BaseBusiness<ChoiceQuestionPaper>();
            db_MachineTestQuestionPaper = new BaseBusiness<MachineTestQuestionPaper>();
            db_examtypes = new BaseBusiness<ExamType>();

        }

        /// <summary>
        /// 所有使用过的选择题
        /// </summary>
        /// <returns></returns>
        public List<ChoiceQuestionPaper> AllChoiceQuestionPaper()
        {

            return db_ChoiceQuestionPaper.GetList().ToList();

        }


        /// <summary>
        /// 所有使用过的解答题题目
        /// </summary>
        /// <returns></returns>
        public List<AnswerQuestionPaper> AllAnswerQuestionPaper()
        {
           return db_AnswerQuestionPaper.GetList();
        }


        /// <summary>
        /// 所有使用过的解答题题目
        /// </summary>
        /// <returns></returns>
        public List<MachineTestQuestionPaper> AllComputerTestQuestionPaper()
        {
            return db_MachineTestQuestionPaper.GetList();
        }

        public List<ExamType> AllexamTypes()
        {
           return db_examtypes.GetList();
        }


    }
}
