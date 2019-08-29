using SiliconValley.InformationSystem.Entity.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Psychro
{
    public class PerInfoBusiness : BaseBusiness<PerInfo>
    {

        /// <summary>
        /// 全部预资详情
        /// </summary>
        /// <returns></returns>
        public List<PerInfo> GetAll()
        {
            return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }
        /// <summary>
        /// 根据预资id返回预资详细对象集合
        /// </summary>
        /// <param name="PerfundingID"></param>
        /// <returns></returns>
        public List<PerInfo> GetPrefundingByID(int PerfundingID)
        {
            return this.GetAll().Where(a => a.PreID == PerfundingID).ToList();
        }
        
        /// <summary>
        /// 添加预资详细
        /// </summary>
        /// <param name="Prefunding"></param>
        public void AddPerfunding(List<PerInfo> perInfos)
        {
            this.Insert(perInfos);
        }
    }
}
