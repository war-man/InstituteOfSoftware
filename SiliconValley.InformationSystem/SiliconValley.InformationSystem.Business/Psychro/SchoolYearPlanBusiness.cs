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
        /// <summary>
        /// 根据计划id获取计划对象
        /// </summary>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public SchoolYearPlan GetPlanByID(int? PlanID) {
            return this.GetAll().Where(a => a.ID == PlanID).FirstOrDefault();
        }

        /// <summary>
        /// 根据当前的年度计划获取下一个年度计划
        /// </summary>
        /// <param name="nowschoolplan"></param>
        /// <returns></returns>
        public SchoolYearPlan GetNextPlan(SchoolYearPlan nowschoolplan)
        {
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

        /// <summary>
        /// 根据当前的年度计划获取上一个年度计划
        /// </summary>
        /// <param name="nowschoolplan"></param>
        /// <returns></returns>
        public SchoolYearPlan GetThePreviousPlan(SchoolYearPlan nowschoolplan)
        {
            //获取年度计划列表
            var planlist = this.GetAll();
            //找到要查询的年度计划对象
            var ThePreviousdata = new SchoolYearPlan();

            for (int i = 0; i < planlist.Count; i++)
            {
                if (planlist[i].ID == nowschoolplan.ID)
                {
                    if (i!=0)
                    {
                        ThePreviousdata = planlist[i - 1];
                        break;
                    }
                }
            }
            return ThePreviousdata;
        }
    }
}
