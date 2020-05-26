using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Psychro
{
    /// <summary>
    /// 区域业务类
    /// </summary>
    public class RegionBusiness : BaseBusiness<Region>
    {
        private ChannelAreaBusiness dbemparea;

        /// <summary>
        /// 获取全部区域
        /// </summary>
        /// <returns></returns>
        public List<Region> GetRegions()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据区域id查找区域对象
        /// </summary>
        /// <param name="Regionid"></param>
        /// <returns></returns>
        public Region GetRegionByID(int Regionid)
        {
            return this.GetRegions().Where(a => a.ID == Regionid).FirstOrDefault();
        }
        /// <summary>
        /// 获取没有分配的区域
        /// </summary>
        
        /// <returns></returns>
        public List<Region> GetNoDistribution()
        {
            dbemparea = new ChannelAreaBusiness();
            var regionlist = this.GetRegions();
            var channelarealist = dbemparea.GetChannelAreas();
            for (int i = regionlist.Count - 1; i >= 0; i--)
            {
                foreach (var item in channelarealist)
                {
                    if (regionlist[i].ID==item.RegionID)
                    {
                        
                        regionlist.Remove(regionlist[i]);
                        i--;
                    }
                }
            }
            return regionlist;
        }

        /// <summary>
        /// 根据员工id返回出这个员工分配的区域
        /// </summary>
        /// <param name="empid"></param>
        /// <returns></returns>
        public List<Region> GetRegionsByempid(int channelid) {
            dbemparea = new ChannelAreaBusiness();
           var data= dbemparea.GetChannelAreas().Where(a => a.ChannelStaffID == channelid).ToList();
            return this.ChannelAreaconversionRegion(data);
        }

        /// <summary>
        ///将员工分配区域对象转化为区域id
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public Region ChannelAreaconversionRegion(ChannelArea param0) {
           return  this.GetEntity(param0.RegionID);
        }

        /// <summary>
        ///将员工分配区域对象转化为区域id
        /// </summary>
        /// <param name="param0"></param>
        /// <returns></returns>
        public List<Region> ChannelAreaconversionRegion(List<ChannelArea> param0)
        {
            List<Region> result = new List<Region>();
            foreach (var item in param0)
            {
                result.Add(this.GetEntity(item.RegionID));
            }
            return result;
        }

        /// <summary>
        /// 根据区域名称获取区域数据
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Region GetSingdata(string name)
        {
           return this.GetList().Where(r => r.RegionName == name).FirstOrDefault();
        }
    }
}
