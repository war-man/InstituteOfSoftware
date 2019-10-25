using SiliconValley.InformationSystem.Entity.Entity;
using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.ViewEntity;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class StockInfoManeger:BaseBusiness<StockInfo>
    {
        //这是物品库存业务类

        public static readonly GoodsTypeManeger GoodsType_Entity = new GoodsTypeManeger();
        public static readonly GoodsManeger Goods_Entity = new GoodsManeger();
        /// <summary>
        /// 获取有效的物品类型集合
        /// </summary>
        /// <returns></returns>
        public List<GoodsType> GetEffective_GoodsTypeData()
        {
            return StockInfoManeger.GoodsType_Entity.GetList().Where(g => g.IsDelete == false).ToList();
        }

        /// <summary>
        /// 获取物品类型表的Tree数据
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public List<TreeClass> GetTreeData(int? id)
        {
            List<GoodsType> type_list = GetEffective_GoodsTypeData().Where(g=>g.FarTher_Id==id).ToList();
            List<TreeClass> tree_list = new List<TreeClass>();
            foreach (GoodsType item in type_list)
            {
                TreeClass new_t = new TreeClass();
                  
                    new_t.title = item.GoodsTypeName;
                    new_t.id = item.Id.ToString();
                    new_t.spread = true;
                    new_t.grade = item.FarTher_Id;
                    new_t.children = GetTreeData(item.Id);
                    tree_list.Add(new_t);                                                            
            }
            return tree_list;
        }

        public List<StockInfo> Get_All_Data()
        {
            return this.GetList();
        }
        /// <summary>
        /// 获取这个Id中的所有子节点包括父级自己
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<GoodsType> Get_child(int id)
        {
           return  GoodsType_Entity.GetList().Where(g => g.FarTher_Id == id || g.Id==id).ToList();
        }
        /// <summary>
        /// 匹配相似名称
        /// </summary>
        /// <param name="name">物品名称</param>
        /// <returns></returns>
        public List<Goods> Get_SomeGoodsData(string name)
        {
           return Goods_Entity.GetList().Where(g => g.GoodsName.Contains(name)).ToList();
        }
        /// <summary>
        /// 获取仓库中所有物品的物品名称
        /// </summary>
        /// <returns></returns>
        public List<ListStockGoods> Get_All_StockGoods(string id)
        {
            List<Goods> g1 = StockInfoManeger.Goods_Entity.GetList();
            List<StockInfo> s1= this.GetList();
            List<ListStockGoods> gg_list = new List<ListStockGoods>();
            foreach (Goods item1 in g1)
            {
                foreach (StockInfo item2 in s1)
                {
                    if (item2.goods_Id == item1.Id)
                    {
                        ListStockGoods list = new ListStockGoods();
                        list.Id = item2.Id;
                        list.GoodsName = item1.GoodsName;
                        gg_list.Add(list);
                    }
                }
            }

            return gg_list;
        }
         
        /// <summary>
        /// 获取物品对应的库存
        /// </summary>
        /// <param name="goodsid">物品Id</param>
        /// <returns></returns>
        public StockInfo GetGoods_Stock(int? goodsid)
        {
           return this.GetList().Where(s => s.goods_Id == goodsid).FirstOrDefault();
        }
        /// <summary>
        /// 改变库存数
        /// </summary>
        /// <param name="stock_Id">库存Id</param>
        /// <param name="count">改变的数量</param>
        /// <param name="IsAdd">true--增加库存,false--减少库存</param>
        /// <returns></returns>
        public bool UpdateCount(int? stock_Id,int? count,bool IsAdd)
        {
            bool s = false;
            try
            {
                StockInfo find_s = this.GetEntity(stock_Id);
                if (IsAdd)
                {
                    //增加库存
                    find_s.stockcount = find_s.stockcount + count;
                }
                else
                {
                    //减少库存   
                    if (find_s.stockcount<count)
                    {
                        s = false;
                        return s;
                    }
                    find_s.stockcount = find_s.stockcount - count;                    
                }
                this.Update(find_s);
                s = true;

            }
            catch (Exception)
            {
                return s;
            }

            return s;
        }
    }
}
