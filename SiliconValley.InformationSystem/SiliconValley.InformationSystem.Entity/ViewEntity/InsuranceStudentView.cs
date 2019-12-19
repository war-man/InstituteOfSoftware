using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class InsuranceStudentView
    {
        public string ID { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string StudentID { get; set; }
        /// <summary>
        /// 学号后面五位数
        /// </summary>
        public string StuID { get;set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 状态 1正常 2过期 3未购买 4未更新
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Context { get; set; }
    }
}
