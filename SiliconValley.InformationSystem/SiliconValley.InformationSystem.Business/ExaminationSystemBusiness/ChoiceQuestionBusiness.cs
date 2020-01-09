using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.ExaminationSystemBusiness
{
    using SiliconValley.InformationSystem.Business.CourseSyllabusBusiness;
    using SiliconValley.InformationSystem.Entity.MyEntity;
    using SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView;
    using SiliconValley.InformationSystem.Business.TeachingDepBusiness;
    using SiliconValley.InformationSystem.Util;


    /// <summary>
    /// 选择题题库业务类
    /// </summary>
    public class ChoiceQuestionBusiness:BaseBusiness<MultipleChoiceQuestion>
    {

        private readonly QuestionLevelBusiness db_questionLevel; 

        private readonly TeacherBusiness db_teacher;

        private readonly BaseBusiness<EmployeesInfo> db_emp;


        /// <summary>
        /// 试卷业务类实例
        /// </summary>
        private readonly ExaminationPaperBusiness db_ExamPaper;

        public ChoiceQuestionBusiness()
        {
            db_questionLevel = new QuestionLevelBusiness();
            db_teacher = new TeacherBusiness();
            db_emp = new BaseBusiness<EmployeesInfo>();
            db_ExamPaper = new ExaminationPaperBusiness();
        }

        /// <summary>
        /// 获取所有选择题题库
        /// </summary>
        /// <returns>选择题集合</returns>
        public List<MultipleChoiceQuestion> AllChoiceQuestionData()
        {

            return this.GetList().ToList();


        }

        /// <summary>
        /// 选择题数据表格实体
        /// </summary>
        /// <param name="multipleChoiceQuestion"></param>
        /// <param name="IsNeedProposition">是否需要阅卷人</param>
        /// <returns></returns>
        public ChoiceQuestionTableView ConvertToChoiceQuestionTableView(MultipleChoiceQuestion multipleChoiceQuestion,bool IsNeedProposition = true)
        {

            CourseBusiness courseBusiness = new CourseBusiness();

            ChoiceQuestionTableView choiceQuestionTableView = new ChoiceQuestionTableView();

            choiceQuestionTableView.Answer = multipleChoiceQuestion.Answer;

            //获取题目所属课程
            choiceQuestionTableView.Course = courseBusiness.GetCurriculas().Where(d => d.CurriculumID == multipleChoiceQuestion.Course).FirstOrDefault();
            choiceQuestionTableView.CreateTime = multipleChoiceQuestion.CreateTime;
            choiceQuestionTableView.Id = multipleChoiceQuestion.Id;
            choiceQuestionTableView.IsRadio = multipleChoiceQuestion.IsRadio;
            choiceQuestionTableView.IsUsing = multipleChoiceQuestion.IsUsing;
            //获取难度级别
            choiceQuestionTableView.Level = db_questionLevel.AllQuestionLevel().Where(d => d.LevelID == multipleChoiceQuestion.Level).FirstOrDefault();
            choiceQuestionTableView.OptionA = multipleChoiceQuestion.OptionA;
            choiceQuestionTableView.OptionB = multipleChoiceQuestion.OptionB;
            choiceQuestionTableView.OptionC = multipleChoiceQuestion.OptionC;
            choiceQuestionTableView.OptionD = multipleChoiceQuestion.OptionD;
            if (IsNeedProposition)
            {
                //获取命题人
                choiceQuestionTableView.Proposition = db_emp.GetList().Where(c => c.EmployeeId == db_teacher.GetTeachers(IsNeedDimission:true).Where(d => d.TeacherID == multipleChoiceQuestion.Proposition).FirstOrDefault().EmployeeId).FirstOrDefault();
            }
            else
            {
                choiceQuestionTableView.Proposition = null;
            }
            

            choiceQuestionTableView.Remark = multipleChoiceQuestion.Remark;
            choiceQuestionTableView.Title = multipleChoiceQuestion.Title;


            return choiceQuestionTableView;
        }


        /// <summary>
        /// 禁用或启用题目
        /// </summary>
        /// <param name="questionIds"></param>
        public void DisableOrEnable(string [] questionIds)
        {

            foreach (var item in questionIds)
            {

                var obj = this.AllChoiceQuestionData().Where(d=>d.Id==int.Parse(item)).FirstOrDefault();

                bool isUsing = obj.IsUsing == true ? false : true;

                obj.IsUsing = isUsing;

                this.Update(obj);

            }



        }


        /// <summary>
        /// 判断选择题是否被使用过
        /// </summary>
        /// <param name="id">选择题ID</param>
        /// <returns></returns>
        public bool IsItUsed(int id)
        {
            var compaerObj = db_ExamPaper.AllChoiceQuestionPaper().Where(d => d.ChooseQuestion == id).FirstOrDefault();

            return compaerObj != null;
        }

        /// <summary>
        /// 删除选择题目
        /// </summary>
        /// <param name="id">选择题ID</param>

        public AjaxResult RemoveChoiceQuestion(int id)
        {

            AjaxResult result = new AjaxResult();

            //被使用的选择题不能被删除

            if (!this.IsItUsed(id))
            {
               var obj = this.AllChoiceQuestionData().Where(d => d.Id == id).FirstOrDefault();

                //可以删除

                try
                {
                    this.Delete(obj);

                    result.ErrorCode = 200;
                    result.Msg = "成功";
                    result.Data = null;
                }
                catch (Exception ex)
                {

                    result.ErrorCode = 500;
                    result.Msg = "失败";
                    result.Data = null;
                }
                


            }
            else
            {
                result.ErrorCode = 400;
                result.Msg = "失败";
                result.Data = null;
            }

            return result;

        }


    }
}
