using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;
   public class StuHomeWorkBusiness:BaseBusiness<HomeWorkFinishRate>
    {

        public List<HomeWorkFinishRate> GetHomeWorkFinishRates()
        {
            return this.GetList().Where(d => d.IsDel == false).ToList();
        }




    }
}
