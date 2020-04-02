using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 返回寝室视图类
    /// </summary>
    public class DormInfoView
    {

        public int ID { get; set; }
        /// <summary>
        /// 宿舍名称
        /// </summary>
        public string DormInfoName { get; set; }
        /// <summary>
        /// 学生名称
        /// </summary>
        public string StudentName { get; set; }
    }
}
