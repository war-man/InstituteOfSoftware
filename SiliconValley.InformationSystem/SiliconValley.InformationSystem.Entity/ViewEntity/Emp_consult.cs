using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 咨询师与员工之间的信息
    /// </summary>
   public class Emp_consult
    {
        /// <summary>
        /// 咨询师编号
        /// </summary>
        public int consultercherid { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        public string empname { get; set; }
    }
}
