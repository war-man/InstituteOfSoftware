using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EmpTransactionBusiness
{
    using SiliconValley.InformationSystem.Entity.Entity;
   public  class MoveTypeManage:BaseBusiness<MoveType>
    {
        /// <summary>
        /// 根据异动类型编号获取异动类型对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MoveType GetmtById(int id) {
            return this.GetEntity(id);
        }
    }
}
