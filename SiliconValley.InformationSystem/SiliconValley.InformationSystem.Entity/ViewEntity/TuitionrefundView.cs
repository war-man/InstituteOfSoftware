using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class TuitionrefundView
    {
        /// <summary>
        /// 退费明目
        /// </summary>
        public int StudentFeeRecordId { get; set; }
        /// <summary>
        /// 退费金额
        /// </summary>
        public decimal Amountofmoney { get; set; }

        public string StudentID
        {
            get; set;
        }
    }
}
