using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class ChannelReportFormView
    {
        public List<ShowEchartsView> ShowEchartsViewData { get; set; }
        public SchoolYearPlan plan;
    }
}
