using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
   public class HealthRegistrationtableview
    {
        /// <summary>
        /// 男/女寝室
        /// </summary>
        public string SexType { get; set; }
        /// <summary>
        ///房间号码
        /// </summary>
        public string DormInfoName { get; set; }

        /// <summary>
        /// 扣分原因
        /// </summary>
        public string Causeofdeduction { get; set; }

        /// <summary>
        /// 登记人
        /// </summary>
        public string EmpinfoName { get; set; }

        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime RecordTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
