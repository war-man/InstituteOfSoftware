using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.ExaminationSystemBusiness
{
    using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView;
    using SiliconValley.InformationSystem.Util;

    /// <summary>
    /// 解答题库业务类
    /// </summary>
    public class AnswerQuestionBusiness:BaseBusiness<AnswerQuestionBank>
    {

        private readonly QuestionLevelBusiness db_questionLevel;

        private readonly TeacherBusiness db_teacher;

        private readonly BaseBusiness<EmployeesInfo> db_emp;

        /// <summary>
        /// 试卷业务类实例
        /// </summary>
        private readonly ExaminationPaperBusiness db_ExamPaper;

        public AnswerQuestionBusiness()
        {
            db_questionLevel = new QuestionLevelBusiness();
            db_emp = new BaseBusiness<EmployeesInfo>();
            db_teacher = new TeacherBusiness();
            db_ExamPaper = new ExaminationPaperBusiness();
        }

        /// <summary>
        /// 获取所有解答题木
        /// </summary>
        /// <returns></returns>
        public List<AnswerQuestionBank> AllAnswerQuestion()
        {
            return this.GetList();
        }


        /// <summary>
        /// 转换视图模型
        /// </summary>
        /// <param name="">AnswerQuestionBank类</param>
        /// <returns></returns>
        public AnswerQuestionView ConvertToAnswerQuestionView(AnswerQuestionBank answerQuestionBank, bool IsNeedProposition)
        {
            AnswerQuestionView answerQuestionView = new AnswerQuestionView();

            CourseBusiness courseBusiness = new CourseBusiness();

            answerQuestionView.Course= courseBusiness.GetCurriculas().Where(d => d.CurriculumID == answerQuestionBank.Course).FirstOrDefault();

            answerQuestionView.ID = answerQuestionBank.ID;
            answerQuestionView.IsUsing = answerQuestionBank.IsUsing;
            answerQuestionView.Level = db_questionLevel.AllQuestionLevel().Where(d => d.LevelID == answerQuestionBank.Level).FirstOrDefault();

            if (IsNeedProposition)
            {
                answerQuestionView.Proposition = db_emp.GetList().Where(c => c.EmployeeId == db_teacher.GetTeachers().Where(d => d.TeacherID == answerQuestionBank.Proposition).FirstOrDefault().EmployeeId).FirstOrDefault();

            }
            else
            {
                answerQuestionView.Proposition = null;
            }

            answerQuestionView.ReferenceAnswer = answerQuestionBank.ReferenceAnswer;
            answerQuestionView.Remark = answerQuestionBank.Remark;
            answerQuestionView.Title = answerQuestionBank.Title;
            answerQuestionView.PropositionDate = answerQuestionBank.PropositionDate;

            return answerQuestionView;
        }



        /// <summary>
        /// 启用或者禁用解答题题目
        /// </summary>
        /// <param name="answerquestionIds"></param>

        public void DisableOrEnable(List<string> answerquestionIds)
        {

            if (answerquestionIds.Count > 0)
            {
                foreach (var item in answerquestionIds)
                {

                    var obj = this.AllAnswerQuestion().Where(d => d.ID == int.Parse(item)).FirstOrDefault() ;

                    bool IsUsing = obj.IsUsing == true ? false : true;

                    obj.IsUsing = IsUsing;

                    this.Update(obj);



                }
            }


        }

        /// <summary>
        /// 判断解答题是否已经被使用
        /// </summary>
        /// <returns></returns>
        public bool IsItUsed(int answerQuestionId)
        {

            var obj = db_ExamPaper.AllAnswerQuestionPaper().Where(d => d.AnswerQuestionID == answerQuestionId).FirstOrDefault();

            return obj != null;
        }

        /// <summary>
        /// 删除解答题题目
        /// </summary>
        /// <returns></returns>
        public AjaxResult Remove(int answerQuestionId)
        {

            AjaxResult result = new AjaxResult();

            //如果已经试卷中被使用则不能删除
            if (!IsItUsed(answerQuestionId))
            {
                var obj = this.AllAnswerQuestion().Where(d => d.ID == answerQuestionId).FirstOrDefault();

                try
                {
                    this.Delete(obj);

                    result.ErrorCode = 200;

                }
                catch (Exception ex)
                {

                    result.ErrorCode = 500;
                }
            }
            else
            {
                result.ErrorCode = 400;
            }

            return result;

        }



        /// <summary>
        /// 筛选解答题题目
        /// </summary>
        /// <returns></returns>
        public List<AnswerQuestionBank> Search(string Title, int course)
        {

            List<AnswerQuestionBank> resultlist = new List<AnswerQuestionBank>();

            if (course == 0 && Title == "")
            {
                resultlist = this.AllAnswerQuestion().ToList();
            }

            if (course != 0 && Title == "")
            {
                resultlist = this.AllAnswerQuestion().Where(d => d.Course == course).ToList();
            }

            if (Title != "" && course != 0)
            {
                resultlist = this.AllAnswerQuestion().Where(d => d.Course == course && d.Title.Contains(Title)).ToList();
            }
            if (Title != "" && course == 0)
            {
                resultlist = this.AllAnswerQuestion().Where(d => d.Title.Contains(Title)).ToList();
            }

            return resultlist;
        }

    }
}
