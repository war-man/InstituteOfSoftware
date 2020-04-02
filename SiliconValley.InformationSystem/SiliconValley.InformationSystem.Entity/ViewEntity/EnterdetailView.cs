using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 学生所在公司或待过的公司
    /// </summary>
    public class EnterdetailView
    {
        /// <summary>
        /// 公司名称
        /// </summary>
        public string EntName { get; set; }
        /// <summary>
        /// 公司地址
        /// </summary>
        public string EntAddress { get; set; }
        /// <summary>
        /// 公司规模
        /// </summary>
        public string EntScale { get; set; }
        /// <summary>
        /// 公司性质
        /// </summary>
        public string EntNature { get; set; }

        /// <summary>
        /// 公司福利
        /// </summary>
        public string EntWelfare { get; set; }
        /// <summary>
        /// 企业备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string EntContacts { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string EntPhone { get; set; }
    }
}
