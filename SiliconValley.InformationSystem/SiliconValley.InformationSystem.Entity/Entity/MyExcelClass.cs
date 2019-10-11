using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Entity.Entity
{
   public class MyExcelClass:IEqualityComparer<MyExcelClass>
    {
       public string StuName { get; set; }
       public string StuSex { get; set; }
       public string StuPhone { get; set; }
       public string StuSchoolName { get; set; }
       public string StuAddress { get; set; }
       public string Region_id { get; set; }
       public string StuInfomationType_Id { get; set; }
       public string StuEducational { get; set; }
       public string EmployeesInfo_Id { get; set; }
       public string Reak { get; set; }

         bool IEqualityComparer<MyExcelClass>.Equals(MyExcelClass x, MyExcelClass y)
        {
            if (x.StuName==y.StuName && y.StuPhone==x.StuPhone)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        int IEqualityComparer<MyExcelClass>.GetHashCode(MyExcelClass obj)
        {
            throw new NotImplementedException();
        }
    }
}
