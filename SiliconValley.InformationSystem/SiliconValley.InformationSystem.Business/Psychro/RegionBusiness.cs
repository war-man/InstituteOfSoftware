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
        /// <param name="dbchannelarea"></param>
        /// <returns></returns>
        public List<Region> GetNoDistribution(ChannelAreaBusiness dbchannelarea)
        {
            var regionlist = this.GetRegions();
            var channelarealist = dbchannelarea.GetChannelAreas();
            for (int i = regionlist.Count - 1; i >= 0; i--)
            {
                foreach (var item in channelarealist)
                {
                    if (regionlist[i].ID==item.RegionID)
                    {
                        regionlist.Remove(regionlist[i]);
                    }
                }
            }
            return regionlist;
        }
    }
}
