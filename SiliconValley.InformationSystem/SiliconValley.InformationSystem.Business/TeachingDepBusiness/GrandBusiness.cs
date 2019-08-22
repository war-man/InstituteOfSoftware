using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    /// <summary>
    /// 阶段 业务类
    /// </summary>
   public class GrandBusiness:BaseBusiness<Grand>
    {

        public Grand GetGrandByID(int GrandID)
        {
           return this.GetList().Where(d=>d.IsDelete==false && d.Id==GrandID).FirstOrDefault();
        }

    }
}
