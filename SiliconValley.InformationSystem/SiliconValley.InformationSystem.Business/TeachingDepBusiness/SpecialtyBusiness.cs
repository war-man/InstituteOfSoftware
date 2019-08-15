using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.TeachingDepBusiness
{
    using SiliconValley.InformationSystem.Entity.MyEntity;

    /// <summary>
    /// 专业业务类
    /// </summary>
   public class SpecialtyBusiness:BaseBusiness<Specialty>
    {

        public Specialty GetSpecialtyByID(int SpecialtyId)
        {

            return this.GetList().Where(t=>t.Id==SpecialtyId).FirstOrDefault();
        }

        public bool IsInList(List<Specialty> souces, Specialty specialty)
        {

            foreach (var item in souces)
            {
                if (item.Id == specialty.Id)
                    return true;
            }

            return false;

        }
    }
}
