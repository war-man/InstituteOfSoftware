using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于年度渠道员工总结下面的圈圈以及右侧的详细介绍
    /// </summary>
    public class MrdCircleRightView
    {
        public MrdCircleView circleView { get; set; }
        public MrdRightView rightView { get; set; }
    }
}
