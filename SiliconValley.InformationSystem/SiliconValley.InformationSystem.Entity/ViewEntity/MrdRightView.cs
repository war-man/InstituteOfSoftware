using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    public class MrdRightView
    {
        public string title { get; set; }

        public List<AmountTopView> top { get; set; }

        public int MarketForecast { get; set; }

        public int BeianCount { get; set; }

        public int GoSchoolCount { get; set; }

        public int BaomingCount { get; set; }

        public string captainname { get; set; }

        public int teamsize { get; set; }

       public string personalempname { get; set; }

        public DateTime personaldate { get; set; }

        public string personalregion { get; set; }
    }
}
