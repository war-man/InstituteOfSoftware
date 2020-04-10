using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
    using SiliconValley.InformationSystem.Entity.Entity;
    /// <summary>
    /// 内训业务类
    /// </summary>
    public class InternalTrainingCostBusiness : BaseBusiness<InternalTrainingCost>
    {
        public List<InternalTrainingCost> internalTrainingCosts(bool IsUsing = true)
        {
            return this.GetList().Where(d => d.IsUsing == IsUsing).ToList();


        }
    }
}
