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
    public class ChannelAreaBusiness : BaseBusiness<ChannelArea>
    {

        
        /// <summary>
        /// 根据区域id获取对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ChannelArea GetAreaByID(int id) {
           return this.GetIQueryable().Where(a => a.ID == id&&a.IsDel==false).FirstOrDefault();
        }
        /// <summary>
        /// 根据渠道员工id获取员工区域
        /// </summary>
        /// <param name="ChannelStaffID"></param>
        /// <returns></returns>
        public List<ChannelArea> GetAreaByChannelID(int ChannelStaffID) {
            return this.GetIQueryable().Where(a => a.ChannelStaffID == ChannelStaffID && a.IsDel == false).ToList();
        }
        /// <summary>
        /// 获取全部的没有伪删除的数据
        /// </summary>
        /// <returns></returns>
        public List<ChannelArea> GetChannelAreas() {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
    }
}
