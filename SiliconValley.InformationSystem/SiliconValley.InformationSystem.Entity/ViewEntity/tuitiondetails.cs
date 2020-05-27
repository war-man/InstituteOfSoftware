using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 缴费详细
    /// </summary>
   public class tuitiondetails
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 费用
        /// </summary>
        public decimal monty { get; set; }

        /// <summary>
        /// ture--缴清，false--未缴清
        /// </summary>
        public bool clean { get; set; }
    }
}
