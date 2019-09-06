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
           var data= this.GetIQueryable().Where(a => a.IsDel == false).ToList();
            return data;
        }
        /// <summary>
        /// 根据计划id获取计划对象
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public SchoolYearPlan GetPlanByID(int? PlanID) {
            return this.GetAll().Where(a => a.ID == PlanID).FirstOrDefault();
        }

        public SchoolYearPlan GetNextPlan(SchoolYearPlan nowschoolplan) {
            //获取年度计划列表
            var planlist = this.GetAll();
            //找到要查询的年度计划对象
            var nextdata = new SchoolYearPlan();

            for (int i = 0; i < planlist.Count; i++)
            {
                if (planlist[i].ID == nowschoolplan.ID)
                {
                    if (i < planlist.Count-1)
                    {
                        nextdata = planlist[i + 1];
                        break;
                    }
                }
            }

            return nextdata;
        }


    }
}
