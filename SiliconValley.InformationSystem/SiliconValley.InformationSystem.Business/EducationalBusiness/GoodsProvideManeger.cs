using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
    using SiliconValley.InformationSystem.Business.EmployeesBusiness;

   public class GoodsProvideManeger:BaseBusiness<GoodsProvide>
    {
        public static readonly EmployeesInfoManage EmployeesInfo_Entity = new EmployeesInfoManage();

        public static readonly StockInfoManeger StockInfo_Entity = new StockInfoManeger();

        public static readonly GoodsManeger Goods_Entity = new GoodsManeger();

        /// <summary>
        /// 根据库存编号查询物品名称
        /// </summary>
        /// <param name="stockinfo_id">库存编号</param>
        /// <returns></returns>
        public Goods GetGoodsName(int? stockinfo_id)
        {
            StockInfo find_s= GoodsProvideManeger.StockInfo_Entity.GetList().Where(s => s.Id == stockinfo_id).FirstOrDefault();
           return  GoodsProvideManeger.Goods_Entity.GetSingleGoods(find_s.goods_Id.ToString(), true);
        }
    }
}
