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
    }
}
