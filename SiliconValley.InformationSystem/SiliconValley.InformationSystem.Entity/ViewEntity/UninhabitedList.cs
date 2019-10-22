using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 未居住列表
    /// </summary>
   public class UninhabitedList
    {
       public List<ProStudentView> proStudentViews { get; set; }
       public List<RoomArrangeEmpinfoView> employeesInfos { get; set; }
    }
}
