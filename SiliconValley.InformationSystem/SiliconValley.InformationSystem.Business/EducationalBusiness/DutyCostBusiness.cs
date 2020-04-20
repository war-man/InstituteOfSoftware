using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    /// <summary>
    /// 阶段课时费业务类
    /// </summary>
    public class DutyCostBusiness : BaseBusiness<BeOnDuty>
    {
        /// <summary>
        /// 获取所有数据
        /// </summary>
        /// <returns></returns>
        public List<BeOnDuty> AllBeOnDuty()
        {
            return this.GetList().ToList();
        }



    }
}
