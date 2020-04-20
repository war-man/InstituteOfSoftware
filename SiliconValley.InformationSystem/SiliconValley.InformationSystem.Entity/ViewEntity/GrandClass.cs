using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 获取属于阶段的所有有效班级
    /// </summary>
   public class GrandClass
    {
        /// <summary>
        /// 阶段名称
        /// </summary>
       public string Grand_Name { get; set; }

        /// <summary>
        /// 班级集合
        /// </summary>
       public List<simpleDataClass> Class_list { get; set; }
    }
}
