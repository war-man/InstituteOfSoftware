using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
    public class WarehouseManeger:BaseBusiness<Warehouse>
    {
        //仓库业务类
        /// <summary>
        /// 根据主键或名称查找单条数据
        /// </summary>
        /// <param name="name">名称或Id</param>
        /// <param name="key">true--根据主键查找 false--根据名称查找</param>
        /// <returns></returns>
        public Warehouse GetSingleData(string name,bool key)
        {
            Warehouse new_w = new Warehouse();
            if (key)
            {
                //按主键查找
                int Id = Convert.ToInt32(name);
                new_w= this.GetEntity(Id);
            }
            else
            {
                new_w= this.GetList().Where(w => w.WarehouseName == name).FirstOrDefault();
            }

            return new_w;
        }

        /// <summary>
        /// 添加方法
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        public bool My_Add(Warehouse w)
        {
            bool s = false;
            try
            {
                this.Insert(w);
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
