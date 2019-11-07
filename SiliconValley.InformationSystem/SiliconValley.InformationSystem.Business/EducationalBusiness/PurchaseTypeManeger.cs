using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class PurchaseTypeManeger:BaseBusiness<PurchaseType>
    {
        //采购类型业务类

        /// <summary>
        /// 根据主键或名称查找单条数据
        /// </summary>
        /// <param name="name">名称或Id</param>
        /// <param name="key">true--根据主键查找 false--根据名称查找</param>
        /// <returns></returns>
        public PurchaseType GetSingleData(string name, bool key)
        {
            PurchaseType new_p = new PurchaseType();
            if (key)
            {
                //按主键查找
                int Id = Convert.ToInt32(name);
                new_p = this.GetEntity(Id);
            }
            else
            {
                new_p = this.GetList().Where(w => w.TypeName == name).FirstOrDefault();
            }

            return new_p;
        }
   
    }
}
