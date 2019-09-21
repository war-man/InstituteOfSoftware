using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    using SiliconValley.InformationSystem.Entity.MyEntity;

    /// <summary>
    /// 满意度调查结果详细视图模型
    /// </summary>
  public  class SatisficingResultDetailView
    {


        public int ID { get; set; }
        public Nullable<int> SatisficingBill { get; set; }
        public SatisficingItem SatisficingItem { get; set; }
        public Nullable<double> Scores { get; set; }
        public string Remark { get; set; }
        
    }
}
