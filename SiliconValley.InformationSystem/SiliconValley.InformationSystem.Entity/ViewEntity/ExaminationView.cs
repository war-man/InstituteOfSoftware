using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SiliconValley.InformationSystem.Entity.MyEntity;
namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 考试信息视图模型
    /// </summary>
  public  class ExaminationView
    {
        public int ID { get; set; }

        public string Title { get; set; }
        public string ExamNo { get; set; }
        /// <summary>
        /// 时限
        /// </summary>
        public int TimeLimit { get; set; }
        /// <summary>
        /// 考试类型
        /// 阶段考试.
        /// 升学考试
        /// </summary>
        public ExamType ExamType { get; set; }
        public string Remark { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BeginDate { get; set; }

        public QuestionLevel PaperLevel { get; set; }
    }
}
