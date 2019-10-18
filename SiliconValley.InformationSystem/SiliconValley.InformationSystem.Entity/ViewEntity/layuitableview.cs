using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 用于返回数据表格类 
    /// </summary>
   public  class layuitableview<T>
    {
     public int code { get; set; }
     public string msg { get; set; }
     public int count { get; set; }
     public List<T> data { get; set; }
    
    }
}
