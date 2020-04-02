using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 加班调休
    /// </summary>
   public  class MyStaticsData
    {
        //员工编号
        public string EmployeeId { get; set; }
        //年份
        public int YearTime { get; set; }
        //加班总时长(小时)
        public Nullable<decimal> OvertimeTotaltime { get; set; }
        //调休总时长(小时)
        public Nullable<decimal> DaysoffTotaltime { get; set; }
        //剩余调休时间
        public Nullable<decimal> ResidueDaysoffTime { get; set; }
       
    }
}
