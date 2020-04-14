using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView
{
    using SiliconValley.InformationSystem.Entity.MyEntity;

    /// <summary>
    /// 机试题视图模型
    /// </summary>
   public class ComputerTestQuestionsView
    {
        public int ID { get; set; }
       /// <summary>
       /// 标题
       /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 保存路径
        /// </summary>
        public string SaveURL { get; set; }
        /// <summary>
        /// 命题人
        /// </summary>
        public EmployeesInfo Proposition { get; set; }
        /// <summary>
        /// 难度级别
        /// </summary>
        public QuestionLevel Level { get; set; }
        public Nullable<bool> IsUsing { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        /// <summary>
        /// 所属课程
        /// </summary>
        public Curriculum Course { get; set; }
        /// <summary>
        /// 使用次数
        /// </summary>
        public int UsageCount { get; set; }
    }
}
