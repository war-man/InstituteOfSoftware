using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    
    using SiliconValley.InformationSystem.Entity.MyEntity;
    /// <summary>
    /// 合作企业信息业务类
    /// </summary>
    public class CooperaEnterprisesBusiness: BaseBusiness<CooperaEnterprises>
    {
        /// <summary>
        /// 根据合作id返回合作对象
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public CooperaEnterprises GetCooByID(int ID) {
            return this.GetIQueryable().Where(a => a.ID == ID && a.IsCooper == true).FirstOrDefault();
        }
        /// <summary>
        /// 根据企业id返回合作对象
        /// </summary>
        /// <param name="EnterID"></param>
        /// <returns></returns>
        public CooperaEnterprises GetCooByEnterID(int? EnterID)
        {
            return this.GetIQueryable().Where(a => a.EnterID == EnterID && a.IsCooper == true).FirstOrDefault();
        }
    }
}
