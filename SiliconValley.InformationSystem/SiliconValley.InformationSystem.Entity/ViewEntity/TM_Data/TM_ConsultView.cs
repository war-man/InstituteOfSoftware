using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.TM_Data
{
    public class TM_ConsultView
    {
        public string StudentName { get; set; }
        public string Phone { get; set; }
        public string Inquiry { get; set; }
        public string MarketType { get; set; }

        public DateTime CreateDate { get; set; }
    }


    public class TM_FlowwView
    {
        public string Remark { get; set; }
        public DateTime CreateDate { get; set; }
        public string StudentName { get; set; }
        public string Phone { get; set; }
        public string Inquiry { get; set; }
    }

}
