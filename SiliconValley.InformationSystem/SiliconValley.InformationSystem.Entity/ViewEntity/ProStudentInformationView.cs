using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于晚归登记显示学生信息
    /// </summary>
  public  class ProStudentInformationView
    {
        public string StudentNmber { get; set; }
        public string SutdentName { get; set; }
        public string ClassNO { get; set; }
        public string DormNO { get; set; }
        public string StuPhone { get; set; }
        public string MasterName { get; set; }
        public string MasterPhone { get; set; }
        public int Count { get; set; }
    }
}
