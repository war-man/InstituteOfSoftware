using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class StudentData
    {             
             public string stuSex { get; set; }
             public string StuName  { get;set; }
             public string StuPhone { get; set; }
             public string StuSchoolName { get; set; }
             public string StuAddress { get; set; }
             public string StuInfomationType_Id { get; set; }
             public string StuStatus_Id { get; set; }
             public string StuIsGoto { get; set; }
             public Nullable<System.DateTime> StuVisit { get; set; }
             public string EmployeesInfo_Id { get; set; }
             public Nullable<System.DateTime> StuDateTime { get; set; }
             public string StuEntering { get; set; }
             public string AreName { get; set; }
    }
}
