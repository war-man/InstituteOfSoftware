using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    public class EmpErrorDataView
    {
        public Nullable<int> ddid { get; set; }//钉钉号
        public string ename { get; set; }//员工名称
        public string errorExplain { get; set; }//部门及岗位的错误
    }
}
