using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ExaminationSystemView
{
   /// <summary>
   /// 考试类型模型视图
   /// </summary>
   public class ExamTypeView
    {
        public int ID { get; set; }
        /// <summary>
        /// 考试类型
        /// </summary>
        public  ExamTypeName TypeName { get; set; }
        /// <summary>
        /// 阶段
        /// </summary>
        public Grand GrandID { get; set; }
    }
}
