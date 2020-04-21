using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于处理一些只需要Id跟Value的结果类
    /// </summary>
   public class simpleDataClass
    {
        public int Id { get; set; }

        public string name { get; set; }
        /// <summary>
        /// 其他字符串类型的Id
        /// </summary>
        public string PID { get; set; }
    }
}
