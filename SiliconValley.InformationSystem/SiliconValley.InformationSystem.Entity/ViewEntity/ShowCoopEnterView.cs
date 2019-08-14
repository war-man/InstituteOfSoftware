using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    public class ShowCoopEnterView
    {
        //企业信息ID
        public int EnterpriseInfoID { get; set; }
        public string EntName { get; set; }
        public string EntAddress { get; set; }
        //规模
        public string EntScale { get; set; }
        //性质
        public string EntNature { get; set; }
        //福利
        public string EntWelfare { get; set; }
        //录入信息人
        public string EmploymentStaffName { get; set; }
        //企业备注
        public string Remark { get; set; }
        //合作企业ID
        public Nullable<int> CooperaEnterprisesID { get; set; }
        //联系人
        public string EntContacts { get; set; }
        //联系方式
        public string EntPhone { get; set; }
        //添加企业时间
        public Nullable<System.DateTime> EntDate { get; set; }
    }
}
