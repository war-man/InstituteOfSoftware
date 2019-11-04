using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
    public class GoodsManeger:BaseBusiness<Goods>
    {
        //这是物品业务类
        
        /// <summary>
        /// 根据Id或名称查询单条数据
        /// </summary>
        /// <param name="name">名称或Id</param>
        /// <param name="key">true--主键查询，false--名称查询</param>
        /// <returns></returns>
        public Goods GetSingleGoods(string name, bool key)
        {
            Goods new_g = new Goods();
            if (key)
            {
                int id = Convert.ToInt32(name);
                new_g = this.GetEntity(id);
            }
            else
            {
                new_g = this.GetList().Where(g => g.GoodsName == name).FirstOrDefault();
            }
            return new_g;
        }
        /// <summary>
        /// 编辑方法
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public bool NewMethod(Goods g)
        {
            bool s = false;
            try
            {
                //编辑
                Goods find_s = this.GetEntity(g.Id);
                if (!string.IsNullOrEmpty(g.GoodsName))
                {
                    find_s.GoodsName = g.GoodsName;
                }                
                find_s.Rmark = g.Rmark;
                find_s.IsReturn = g.IsReturn;
                this.Update(find_s);
                s = true;
            }
            catch (Exception)
            {

                s = false;
            }
            return s;
        }
    }
}
