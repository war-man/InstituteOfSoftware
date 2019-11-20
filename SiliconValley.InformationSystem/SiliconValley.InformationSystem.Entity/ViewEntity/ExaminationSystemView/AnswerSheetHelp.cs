using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView
{
    /// <summary>
    /// 此类在提供阅卷数据时候使用
    /// </summary>
   public class AnswerSheetHelp
    {
        /// <summary>
        /// 解答题ID
        /// </summary>
        public int questionid { get; set; }

        /// <summary>
        /// 学员填写的答案
        /// </summary>

        public string answer { get; set; }

        /// <summary>
        /// 题目分数
        /// </summary>

        public float questionScores { get; set; }
    }
}
