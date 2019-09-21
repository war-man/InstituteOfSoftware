using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于显示top
    /// </summary>
    public class AmountTopView
    {
        public string countname { get; set; }

        public string top1name { get; set; }
        public int top1count { get; set; }
        public string top2name { get; set; }
        public int top2count { get; set; }
        public string top3name { get; set; }
        public int top3count { get; set; }
    }
}
