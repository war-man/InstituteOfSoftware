using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView
{
    /// <summary>
    /// 重复访谈记录
    /// </summary>
   public class DuplicateSurveyRecords
    {
        public string StudentNumber { get; set; }
        public List<SurveyRecords> Duplicatedata { get; set; }
    }
}
