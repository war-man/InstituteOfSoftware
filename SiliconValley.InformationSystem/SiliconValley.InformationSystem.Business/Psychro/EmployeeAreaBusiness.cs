using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Psychro
{
    /// <summary>
    /// 渠道专员区域分布
    /// </summary>
    public class EmployeeAreaBusiness : BaseBusiness<EmployeeArea>
    {
        /// <summary>
        /// 根据区域id获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EmployeeArea GetAreaByID(int id) {
           return this.GetIQueryable().Where(a => a.ID == id).FirstOrDefault();
        }

        
    }
}
