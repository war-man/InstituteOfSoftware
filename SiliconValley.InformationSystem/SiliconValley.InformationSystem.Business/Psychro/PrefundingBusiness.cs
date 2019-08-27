using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Psychro
{
    public class PrefundingBusiness : BaseBusiness<Prefunding>
    {
        /// <summary>
        /// 全部预资数据
        /// </summary>
        /// <returns></returns>
        public List<Prefunding> GetAll() {
          return  this.GetIQueryable().Where(a=>a.IsDel==false).ToList();
        }
        /// <summary>
        /// 根据预资id返回预资单对象
        /// </summary>
        /// <param name="PerfundingID"></param>
        /// <returns></returns>
        public Prefunding GetPrefundingByID(int PerfundingID) {
            return this.GetAll().Where(a => a.ID == PerfundingID).FirstOrDefault();
        }
        /// <summary>
        /// 根据渠道员工id返回他的所有借资单集合
        /// </summary>
        /// <param name="ChannelStaffID"></param>
        /// <returns></returns>
        public List<Prefunding> GetPrefundingByChannelStaffID(int ChannelStaffID) {
            return this.GetAll().Where(a => a.ChannelStaffID == ChannelStaffID).ToList();
        }

        /// <summary>
        /// 添加预资单
        /// </summary>
        /// <param name="Prefunding"></param>
        public void AddPerfunding(Prefunding Prefunding) {
            this.Insert(Prefunding);
        }
    }
}
