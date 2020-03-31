using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Entity.ViewEntity
{
    /// <summary>
    /// 编辑企业视图
    /// </summary>
    public class EditEnterpriseView
    {
        /// <summary>
        /// 公司id
        /// </summary>
        public int EntID { get; set; }
        /// <summary>
        /// 操作标号
        /// </summary>
        public int OperNO { get; set; }
        public string EntName { get; set; }

        public string EntAddress { get; set; }
        /// <summary>
        ///  规模
        /// </summary>
        public string EntScale { get; set; }
        /// <summary>
        ///   性质
        /// </summary>
        public string EntNature { get; set; }

        /// <summary>
        ///   福利
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
