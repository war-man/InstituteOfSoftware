using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 排课数据
    /// </summary>
   public class AnPaiData
    {
        /// <summary>
        /// 班级Id
        /// </summary>
        public int class_Id
        {
            get;set;
        }
        /// <summary>
        /// 班级名称
        /// </summary>
        public string ClassName
        {
            get;set;
        }
        /// <summary>
        /// 上课内容
        /// </summary>
        public string NeiRong
        {
            get;set;
        }
        /// <summary>
        /// 任课老师
        /// </summary>
        public string Teacher
        {
            get;set;
        }
    }
}
