using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView
{

    /// <summary>
    /// 年度计划页面model
    /// </summary>
   public class QuarterView
    {
        public int ID { get; set; }
        public string QuaTitle { get; set; }
        public string Remark { get; set; }
        public DateTime RegDate { get; set; }

        public object EmpQuarterClassList { get; set; }
    }
}
