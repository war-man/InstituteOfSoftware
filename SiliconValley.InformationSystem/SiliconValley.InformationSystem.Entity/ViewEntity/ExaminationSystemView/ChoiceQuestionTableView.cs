using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView
{
    using SiliconValley.InformationSystem.Entity.MyEntity;

    /// <summary>
    /// 选择题表格实体
    /// </summary>
    public class ChoiceQuestionTableView
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 题目
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 选项A
        /// </summary>
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }

        /// <summary>
        /// 是否为单选
        /// </summary>
        public Nullable<bool> IsRadio { get; set; }
        /// <summary>
        /// 难度级别
        /// </summary>
        public QuestionLevel Level { get; set; }
        /// <summary>
        /// 答案
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 命题人
        /// </summary>
        public EmployeesInfo Proposition { get; set; }
        /// <summary>
        /// 命题时间
        /// </summary>
        public Nullable<System.DateTime> CreateTime { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public Nullable<bool> IsUsing { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 所属课程
        /// </summary>
        public Curriculum Course { get; set; }
    }
}
