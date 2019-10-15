using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.EducationalBusiness
{
   public class BaseDataEnumManeger:BaseBusiness<BaseDataEnum>
    {
        /// <summary>
        /// 根据主键或名称查询数据
        /// </summary>
        /// <param name="name">主键或名称（名称不能模糊查询）</param>
        /// <param name="key">true--按照主键查询，false---按照名称查询</param>
        /// <returns></returns>
        public BaseDataEnum GetSingData(string name,bool key)
        {
            BaseDataEnum b_new = new BaseDataEnum();
            if (key)
            {
                int id = Convert.ToInt32(name);
                b_new = this.GetEntity(id);
            }
            else
            {
                b_new= this.GetList().Where(b => b.Name == name).FirstOrDefault();
            }
            return b_new;
        }
    }
}
