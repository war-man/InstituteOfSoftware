using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.DormitoryBusiness
{
     public class BenNumberBusiness:BaseBusiness<BenNumber>
    {
        public List<BenNumber> GetBenNumbers() {
            return this.GetIQueryable().Where(a => a.IsDelete == false).ToList();
        }

        public BenNumber GetBenByBenID(int BenID) {
            return this.GetBenNumbers().Where(a => a.Id == BenID).FirstOrDefault();
        }

        /// <summary>
        /// 根据床位获取前数量
        /// </summary>
        /// <param name="bennumber"></param>
        /// <returns></returns>
        public List<BenNumber> Getbennumber(int bennumber) {
            return this.GetBenNumbers().Take(bennumber).ToList();
        }
        
    }
}
