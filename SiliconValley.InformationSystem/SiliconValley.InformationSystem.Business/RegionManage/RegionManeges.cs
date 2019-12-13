using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconValley.InformationSystem.Entity.MyEntity;

namespace SiliconValley.InformationSystem.Business.RegionManage
{
    public class RegionManeges:BaseBusiness<Region>
    {  
        /// <summary>
        /// 查找单条数据
        /// </summary>
        /// <param name="id">值</param>
        /// <param name="isKey">true--根据主键查询，false--根据name查询</param>
        /// <returns></returns>
        public Region SerchRegionName(string id,bool isKey)
        {
            Region R = new Region();
            if (isKey && !string.IsNullOrEmpty(id))
            {
                //是主键
                int Id = Convert.ToInt32(id);
                R= this.GetEntity(Id);
            }
            else
            {
                //不是主键
               R= this.GetList().Where(r => r.RegionName == id).FirstOrDefault();
            }
            if (R==null)
            {
                R.RegionName = "区域外";
            }
            return R;          
        }

        /// <summary>
        /// 获取Id或者获取名称
        /// </summary>
        /// <param name="Id">Id或者名称</param>
        /// <param name="IsKey">false:根据名称找Id,true:根据ID找Name</param>
        /// <returns></returns>
        public Region GetAreValue(string Id, bool IsKey)
        {
            if (IsKey && !string.IsNullOrEmpty(Id))
            {                
                return  this.GetEntity(Convert.ToInt32(Id));                
            }
            else
            {
               return this.GetList().Where(r => r.RegionName == Id).FirstOrDefault();//通过名称找主键                 
            }
        }
    }
}
