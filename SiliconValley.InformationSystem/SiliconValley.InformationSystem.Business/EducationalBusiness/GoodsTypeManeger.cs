using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class GoodsTypeManeger:BaseBusiness<GoodsType>
    {
        //这是物品类型业务类

        /// <summary>
        /// 根据Id或名称查询单条数据
        /// </summary>
        /// <param name="name">名称或Id</param>
        /// <param name="key">true--主键查询，false--名称查询</param>
        /// <returns></returns>
        public GoodsType GetSingleGoodsType(string name,bool key)
        {
            GoodsType new_g = new GoodsType();
            if (key)
            {
                int id = Convert.ToInt32(name);
                new_g= this.GetEntity(id);
            }
            else
            {
                new_g= this.GetList().Where(g => g.GoodsTypeName == name).FirstOrDefault();
            }
            return new_g;
        }
    }
}
