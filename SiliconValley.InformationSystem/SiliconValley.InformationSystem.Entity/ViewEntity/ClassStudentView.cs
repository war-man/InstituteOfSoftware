using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
  public  class ClassStudentView
    {
        /// <summary>
        /// 学员学号
        /// </summary>
        public string StuNameID { get; set; }//
        /// <summary>
        /// 学员姓名
        /// </summary>
        public string Name { get; set; }//
        /// <summary>
        /// 班委名称
        /// </summary>
        public object Nameofmembers { get; set; }//
    }
}
