using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView
{
    using SiliconValley.InformationSystem.Entity.MyEntity;

    /// <summary>
    /// 解答题视图
    /// </summary>
   public class AnswerQuestionView
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 参考答案
        /// </summary>
        public string ReferenceAnswer { get; set; }
        /// <summary>
        /// 所属课程
        /// </summary>
        public Curriculum Course { get; set; }
        /// <summary>
        /// 命题人
        /// </summary>
        public EmployeesInfo Proposition { get; set; }

        /// <summary>
        /// 难度级别
        /// </summary>
        public QuestionLevel Level { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public Nullable<bool> IsUsing { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
       /// <summary>
       /// 命题日期
       /// </summary>
        public Nullable<DateTime> PropositionDate { get; set; }

        public int Grand { get; set; }
    }
}
