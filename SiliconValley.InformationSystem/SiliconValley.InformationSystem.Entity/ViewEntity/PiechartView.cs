using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 饼图实体
    /// </summary>
    public class PiechartView
    {
        /// <summary>
        /// 数量
        /// </summary>
        public int count { get; set; }
        /// <summary>
        /// 颜色可以是16进制 也可以是 red 这样的单词
        /// </summary>
        public string Corlor { get; set; }
        /// <summary>
        /// 显示i的字符串
        /// </summary>
        public string showname { get; set; }
    }
}
