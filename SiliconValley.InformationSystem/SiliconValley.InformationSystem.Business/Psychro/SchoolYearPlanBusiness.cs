using SiliconValley.InformationSystem.Entity.MyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiliconValley.InformationSystem.Business.Psychro
{
    /// <summary>
    /// 年度计划业务类
    /// </summary>
    public  class SchoolYearPlanBusiness:BaseBusiness<SchoolYearPlan>
    {
        /// <summary>
        /// 获取计划安排。
        /// </summary>
        /// <returns></returns>
        public List<SchoolYearPlan> GetAll() {
           return this.GetIQueryable().Where(a => a.IsDel == false).ToList();
        }

    }
}
