using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Psychro
{
    /// <summary>
    /// 渠道员工分配计划的业务类
    /// </summary>
    public class ChannelYearPlanBusiness : BaseBusiness<ChannelYearPlan>
    {
        /// <summary>
        /// 根据学校年度计划id查询渠道员工年度计划列表
        /// </summary>
        /// <param name="schoolplanid"></param>
        /// <returns></returns>
        public List<ChannelYearPlan> GetChannelYearPlansBySchoolplanID(int schoolplanid)
        {
            return this.GetIQueryable().Where(a => a.SchoolYearPlanID == schoolplanid).ToList();
        }
    }
}
