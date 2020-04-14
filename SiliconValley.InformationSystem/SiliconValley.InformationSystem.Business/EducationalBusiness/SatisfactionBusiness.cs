using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
    /// <summary>
    /// 满意度系数业务类
    /// </summary>
    public class SatisfactionBusiness:BaseBusiness<Satisfaction>
    {
        public List<Satisfaction> satisfacctions(bool IsDelete = false)
        {
            return this.GetList().Where(d => d.IsDelete == IsDelete).ToList();
        }
    }
}
