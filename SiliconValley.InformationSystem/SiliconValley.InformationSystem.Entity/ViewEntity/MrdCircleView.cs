using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    public class MrdCircleView
    {
        public bool isExistData { get; set; }
        public bool isMonth { get; set; }
        public float beianRatio { get; set; }
        public float goSchoolRatio { get; set; }
        public float baomingRatio { get; set; }

        public float beianCount { get; set; }
        public float goSchoolCount { get; set; }
        public float baomingCount { get; set; }
        public MrdCircleView()
        {
            this.isExistData = false;
            this.isMonth = false;
        }
    }
}
