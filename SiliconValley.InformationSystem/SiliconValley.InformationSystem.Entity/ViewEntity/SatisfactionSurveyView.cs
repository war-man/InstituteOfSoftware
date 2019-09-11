using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    using SiliconValley.InformationSystem.Entity.MyEntity;


    /// <summary>
    /// 满意度调查具体项视图模型
    /// </summary>
   public class SatisfactionSurveyView
    {

        public int ItemID { get; set; }
        public string ItemContent { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDel { get; set; }
        public SatisficingType ItemType { get; set; }

    }
}
