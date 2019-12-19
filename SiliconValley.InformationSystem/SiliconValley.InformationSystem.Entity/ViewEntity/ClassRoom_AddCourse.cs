using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 加课所需的教室实体类
    /// </summary>
   public class ClassRoom_AddCourse
    {
        /// <summary>
        /// （教室编号）
        /// </summary>
        public int ClassRoomId { get; set; }
        /// <summary>
        /// (晚一，晚二)
        /// </summary>
        public string TimeName { get; set; }
    }
}
