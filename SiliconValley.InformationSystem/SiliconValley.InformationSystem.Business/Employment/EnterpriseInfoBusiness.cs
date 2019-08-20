using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Employment
{
    /// <summary>
    /// 企业信息业务类
    /// </summary>
    public class EnterpriseInfoBusiness:BaseBusiness<EnterpriseInfo>
    {
        /// <summary>
        /// 根据id查找该id对应的对象
        /// </summary>
        /// <param name="EnterID"></param>
        /// <returns></returns>
        public EnterpriseInfo GetEnterByID(int? EnterID) {
            return this.GetIQueryable().Where(a => a.ID == EnterID).FirstOrDefault();
        }
    }
}
