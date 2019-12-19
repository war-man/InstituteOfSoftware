using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity.ObtainEmploymentView
{
    /// <summary>
    /// 用户分配返回服务器的model
    /// </summary>
    public class addEmpStaffAndStuView
    {
        public int EmpStaffID { get; set; }
        public List<QuarterIDAndStudentno> quarterIDAndStudentnos { get; set; }
    }
}
